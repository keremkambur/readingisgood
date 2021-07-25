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
using ReadingIsGood.EntityLayer.Database;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.DataLayer.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        internal DbContext DbContext { get; }
        public bool BulkMode { get; private set; }
        protected bool HasUnsavedChanges => this.DbContext?.ChangeTracker.HasChanges() ?? false;

        public DatabaseRepository(ILogger logger, DbContext dbContext)   
        {
            this.DbContext = dbContext;
            
            this.UserCrudOperations = new CrudOperations<User>(this);
            this.ProductOrderCrudOperations = new CrudOperations<Product>(this);
            this.OrderCrudOperations = new CrudOperations<Order>(this);
        }

        public ICrudOperations<Order> OrderCrudOperations { get; set; }

        public ICrudOperations<User> UserCrudOperations { get; set; }

        public ICrudOperations<Product> ProductOrderCrudOperations { get; set; }

        public void EnableBulkMode() => this.BulkMode = true;

        public void DisableBulkMode(bool saveChanges = false)
        {
            this.BulkMode = false;

            if (this.HasUnsavedChanges)
            {
                this.CommitChanges();
            }
        }

        public async Task LoadReference<TEntry, TProperty>(TEntry entity, Expression<Func<TEntry, TProperty>> expression, CancellationToken cancellationToken)
            where TEntry : class, IEntity
            where TProperty : class
            => await this.DbContext.Entry(entity).Reference(expression).LoadAsync(cancellationToken).ConfigureAwait(false);

        public async Task LoadCollection<TEntry, TProperty>(TEntry entity, Expression<Func<TEntry, IEnumerable<TProperty>>> expression, CancellationToken cancellationToken)
            where TEntry : class, IEntity
            where TProperty : class
            => await this.DbContext.Entry(entity).Collection(expression).LoadAsync(cancellationToken).ConfigureAwait(false);

        /// <inheritdoc />
        public void Detach<TEntry>(TEntry entity)
        {
            var entry = this.DbContext.Entry(entity);

            foreach (var navigationEntry in entry.Navigations)
            {
                if (navigationEntry.CurrentValue is IEnumerable<IEntity> children)
                {
                    foreach (var child in children)
                    {
                        this.DbContext.Entry(child).State = EntityState.Detached;
                    }
                }
                else if (entry.CurrentValues is IEntity child)
                {
                    this.DbContext.Entry(child).State = EntityState.Detached;
                }
            }

            entry.State = EntityState.Detached;
        }

        /// <inheritdoc />
        public async Task<TEntry> Reload<TEntry>(TEntry entity)
        {
            var entry = this.DbContext.Entry(entity);
            await entry.ReloadAsync();

            return (TEntry)entry.Entity;
        }

        internal virtual TEntity AddEntity<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            this.DbContext.Set<TEntity>().Add(entity);

            this.CommitChanges();

            return entity;
        }

        internal virtual TEntity DeleteEntity<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (entity == null)
            {
                return null;
            }

            this.DbContext.Set<TEntity>().Remove(entity);

            this.CommitChanges();

            return entity;
        }

        internal virtual TEntity UpdateEntity<TEntity>(TEntity changes) where TEntity : class, IEntity
        {
            this.CommitChanges();

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
            foreach (var entry in this.DbContext.ChangeTracker.Entries())
            {
                if (!(entry.Entity is IAuditEntity))
                {
                    continue;
                }

                if (!(entry.Entity is IEntity entity))
                {
                    continue;
                }

                if (entry.State != EntityState.Modified && entry.State != EntityState.Deleted)
                {
                    continue;
                }

                var batchId = Guid.NewGuid();
                foreach (var property in entry.Properties)
                {
                    // ignore unchanged properties from modified entities
                    if (entry.State == EntityState.Modified
                        && string.Concat(property.CurrentValue) == string.Concat(property.OriginalValue))
                    {
                        continue;
                    }

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

        public int CommitChanges()
        {
            if (this.BulkMode)
            {
                return 0;
            }

            this.DbContext
                .Set<ChangeLog>()
                .AddRange(this.GetChanges().ToList());

            this.UpdateIAuditEntities();

            return this.DbContext.SaveChanges();
        }

        private void UpdateIAuditEntities()
        {
            foreach (var entry in this.DbContext.ChangeTracker.Entries())
            {
                if (!(entry.Entity is IAuditEntity entity))
                {
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        this.Add(entity);
                        break;

                    case EntityState.Modified:
                        this.Update(entity);
                        break;

                    case EntityState.Deleted:
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                    default:
                        continue;
                }
            }
        }

        public Task<int> CommitChangesAsync()
        {
            if (this.BulkMode)
            {
                return Task.FromResult(0);
            }

            this.DbContext
                .Set<ChangeLog>()
                .AddRange(this.GetChanges().ToList());

            return this.DbContext.SaveChangesAsync();
        }

        public virtual bool Any<TEntity>() where TEntity : class, IEntity
            => this.DbContext
                .Set<TEntity>()
                .Any();

        public virtual bool Any<TEntity>(Func<TEntity, bool> predicate) where TEntity : class, IEntity
            => this.DbContext
                .Set<TEntity>()
                .Any(predicate);

        public virtual TEntity Find<TEntity>(params object[] keyValues) where TEntity : class, IEntity
            => this.DbContext.Find<TEntity>(keyValues);

        public virtual TEntity QueryEntity<TEntity>(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity
        {
            if (includes == null || includes.Length == 0)
            {
                return this.DbContext
                    .Set<TEntity>()
                    .SingleOrDefault(predicate);
            }

            var query = this.DbContext.Set<TEntity>().AsQueryable();

            return includes
                .Aggregate(query, (current, include) => current.Include(include))
                .SingleOrDefault(predicate);
        }

        public virtual IQueryable<TEntity> QueryEntities<TEntity>(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity
        {
            if (includes == null || includes.Length == 0)
            {
                return this.DbContext
                    .Set<TEntity>()
                    .Where(predicate)
                    .AsQueryable();
            }

            var query = this.DbContext.Set<TEntity>().AsQueryable();

            return includes
                .Aggregate(query, (current, include) => current.Include(include))
                .Where(predicate)
                .AsQueryable();

        }

        public virtual IEnumerable<TEntity> GetEntities<TEntity>(int pageSize = 0, int pageNumber = 0, params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity
        {
            var query = this.DbContext.Set<TEntity>().AsQueryable();

            if (includes != null && includes.Length != 0)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return pageSize == 0 && pageNumber == 0
                ? query.ToList()
                : query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity
            => this.DbContext.Attach(entity);

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}