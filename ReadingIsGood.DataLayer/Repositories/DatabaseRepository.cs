using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadingIsGood.DataLayer.Contracts;
using ReadingIsGood.DataLayer.Extensions;
using ReadingIsGood.EntityLayer.Database.Base;
using ReadingIsGood.EntityLayer.Database.Content;
using ReadingIsGood.EntityLayer.Database.System;

namespace ReadingIsGood.DataLayer.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        public DatabaseRepository(ILogger logger, DbContext dbContext)
        {
            DbContext = dbContext;

            ProductCrudOperations = new CrudOperations<Product>(this);
            OrderCrudOperations = new CrudOperations<Order>(this);
        }

        internal DbContext DbContext { get; }
        protected bool HasUnsavedChanges => DbContext?.ChangeTracker.HasChanges() ?? false;
        public bool BulkMode { get; private set; }

        public ICrudOperations<Order> OrderCrudOperations { get; set; }

        public ICrudOperations<Product> ProductCrudOperations { get; set; }

        public void EnableBulkMode()
        {
            BulkMode = true;
        }

        public void DisableBulkMode(bool saveChanges = false)
        {
            BulkMode = false;

            if (HasUnsavedChanges) CommitChanges();
        }

        public async Task LoadReference<TEntry, TProperty>(TEntry entity,
            Expression<Func<TEntry, TProperty>> expression, CancellationToken cancellationToken)
            where TEntry : class, IEntity
            where TProperty : class
        {
            await DbContext.Entry(entity).Reference(expression).LoadAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task LoadCollection<TEntry, TProperty>(TEntry entity,
            Expression<Func<TEntry, IEnumerable<TProperty>>> expression, CancellationToken cancellationToken)
            where TEntry : class, IEntity
            where TProperty : class
        {
            await DbContext.Entry(entity).Collection(expression).LoadAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void Detach<TEntry>(TEntry entity)
        {
            var entry = DbContext.Entry(entity);

            foreach (var navigationEntry in entry.Navigations)
                if (navigationEntry.CurrentValue is IEnumerable<IEntity> children)
                    foreach (var child in children)
                        DbContext.Entry(child).State = EntityState.Detached;
                else if (entry.CurrentValues is IEntity child) DbContext.Entry(child).State = EntityState.Detached;

            entry.State = EntityState.Detached;
        }

        /// <inheritdoc />
        public async Task<TEntry> Reload<TEntry>(TEntry entity)
        {
            var entry = DbContext.Entry(entity);
            await entry.ReloadAsync();

            return (TEntry) entry.Entity;
        }

        public int CommitChanges()
        {
            if (BulkMode) return 0;

            DbContext
                .Set<ChangeLog>()
                .AddRange(GetChanges().ToList());

            UpdateIAuditEntities();

            return DbContext.SaveChanges();
        }

        public Task<int> CommitChangesAsync()
        {
            if (BulkMode) return Task.FromResult(0);

            DbContext
                .Set<ChangeLog>()
                .AddRange(GetChanges().ToList());

            return DbContext.SaveChangesAsync();
        }

        public virtual bool Any<TEntity>() where TEntity : class, IEntity
        {
            return DbContext
                .Set<TEntity>()
                .Any();
        }

        public virtual bool Any<TEntity>(Func<TEntity, bool> predicate) where TEntity : class, IEntity
        {
            return DbContext
                .Set<TEntity>()
                .Any(predicate);
        }

        public virtual TEntity Find<TEntity>(params object[] keyValues) where TEntity : class, IEntity
        {
            return DbContext.Find<TEntity>(keyValues);
        }

        public virtual TEntity QueryEntity<TEntity>(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity
        {
            if (includes == null || includes.Length == 0)
                return DbContext
                    .Set<TEntity>()
                    .SingleOrDefault(predicate);

            var query = DbContext.Set<TEntity>().AsQueryable();

            return includes
                .Aggregate(query, (current, include) => current.Include(include))
                .SingleOrDefault(predicate);
        }

        public virtual IQueryable<TEntity> QueryEntities<TEntity>(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity
        {
            if (includes == null || includes.Length == 0)
                return DbContext
                    .Set<TEntity>()
                    .Where(predicate)
                    .AsQueryable();

            var query = DbContext.Set<TEntity>().AsQueryable();

            return includes
                .Aggregate(query, (current, include) => current.Include(include))
                .Where(predicate)
                .AsQueryable();
        }

        public virtual IEnumerable<TEntity> GetEntities<TEntity>(int pageSize = 0, int pageNumber = 0,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity
        {
            var query = DbContext.Set<TEntity>().AsQueryable();

            if (includes != null && includes.Length != 0)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return pageSize == 0 && pageNumber == 0
                ? query.ToList()
                : query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            DbContext.Attach(entity);
        }

        public virtual void Dispose()
        {
            DbContext?.Dispose();
        }

        internal virtual TEntity AddEntity<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            DbContext.Set<TEntity>().Add(entity);

            CommitChanges();

            return entity;
        }

        internal virtual TEntity DeleteEntity<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (entity == null) return null;

            DbContext.Set<TEntity>().Remove(entity);

            CommitChanges();

            return entity;
        }

        internal virtual TEntity UpdateEntity<TEntity>(TEntity changes) where TEntity : class, IEntity
        {
            CommitChanges();

            return changes;
        }

        protected virtual void Add(IAuditEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }

        protected virtual void Update(IAuditEntity entity)
        {
            entity.ModifiedAt = DateTime.UtcNow;
        }

        protected virtual IEnumerable<ChangeLog> GetChanges()
        {
            foreach (var entry in DbContext.ChangeTracker.Entries())
            {
                if (!(entry.Entity is IAuditEntity)) continue;

                if (!(entry.Entity is IEntity entity)) continue;

                if (entry.State != EntityState.Modified && entry.State != EntityState.Deleted) continue;

                var batchId = Guid.NewGuid();
                foreach (var property in entry.Properties)
                {
                    // ignore unchanged properties from modified entities
                    if (entry.State == EntityState.Modified
                        && string.Concat(property.CurrentValue) == string.Concat(property.OriginalValue))
                        continue;

                    yield return new ChangeLog
                    {
                        BatchUuid = batchId,
                        CreatedAt = DateTime.UtcNow,
                        Application = entry.Metadata.ClrType.Assembly.FullName,
                        ObjectId = entity.Id,
                        ObjectUuid = entity.Uuid,
                        ClassName = entry.Entity.GetType().FullName,
                        OriginalValue = property.OriginalValue?.ToString() ?? string.Empty,
                        CurrentValue = property.CurrentValue?.ToString() ?? string.Empty,
                        PropertyName = property.Metadata.Name,
                        PropertyType = property.Metadata.ClrType.GetFriendlyName(),
                        State = entry.State.ToString()
                    };
                }
            }
        }

        private void UpdateIAuditEntities()
        {
            foreach (var entry in DbContext.ChangeTracker.Entries())
            {
                if (!(entry.Entity is IAuditEntity entity)) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        Add(entity);
                        break;

                    case EntityState.Modified:
                        Update(entity);
                        break;

                    case EntityState.Deleted:
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                    default:
                        continue;
                }
            }
        }
    }
}