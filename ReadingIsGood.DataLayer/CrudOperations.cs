using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.DataLayer.Contracts;
using ReadingIsGood.DataLayer.Extensions;
using ReadingIsGood.DataLayer.Repositories;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.DataLayer
{
    public class CrudOperations<TModel> : ICrudOperations<TModel> where TModel : class, IEntity
    {
        public CrudOperations(DatabaseRepository repository)
        {
            Repository = repository;
        }

        public DatabaseRepository Repository { get; }

        public TModel Create(TModel entity)
        {
            return Repository.AddEntity(entity);
        }

        public IEnumerable<TModel> Read(params Expression<Func<TModel, object>>[] includes)
        {
            return Repository.GetEntities(includes: includes);
        }

        public TModel Read(int id, params Expression<Func<TModel, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
                return Repository.DbContext
                    .Set<TModel>()
                    .Find(id);

            var query = Repository.DbContext.Set<TModel>().AsQueryable();

            return includes
                .Aggregate(query, (current, include) => current.Include(include))
                .SingleOrDefault(item => item.Id == id);
        }

        public TModel Read(Guid uuid, params Expression<Func<TModel, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
                return Repository.DbContext
                    .Set<TModel>()
                    .SingleOrDefault(item => item.Uuid == uuid);

            var query = Repository.DbContext.Set<TModel>().AsQueryable();

            return includes
                .Aggregate(query, (current, include) => current.Include(include))
                .SingleOrDefault(item => item.Uuid == uuid);
        }

        public List<TModel> QueryList(Func<TModel, bool> predicate, int? skip = null, int? take = null,
            params Expression<Func<TModel, object>>[] includes)
        {
            var query = Repository.QueryEntities(predicate, includes);

            if (skip.HasValue) query = query.Skip(skip.Value);

            if (take.HasValue) query = query.Take(take.Value);

            return query.ToList();
        }

        public TModel QuerySingle(Func<TModel, bool> predicate, params Expression<Func<TModel, object>>[] includes)
        {
            return Repository.QueryEntity(predicate, includes);
        }

        public DbSet<TModel> QueryDbSet()
        {
            return Repository
                .DbContext
                .Set<TModel>();
        }

        public TModel Update(Guid uuid, Action<TModel> updateCallback)
        {
            var entity = Read(uuid);

            if (entity == null) return null;

            updateCallback(entity);

            return Repository.UpdateEntity(entity);
        }

        public async Task<TModel> Update(Guid uuid, Func<TModel, Task> updateCallback)
        {
            var entity = Read(uuid);

            if (entity == null) return null;

            await updateCallback(entity);

            return Repository.UpdateEntity(entity);
        }

        /// <inheritdoc />
        public TModel Delete(int id)
        {
            return Delete(Read(id));
        }

        public TModel Delete(Guid uuid)
        {
            return Delete(Read(uuid));
        }

        public TModel Delete(TModel entity)
        {
            return Repository.DeleteEntity(entity);
        }

        public List<TModel> ReadByCreateDateTime(DateTime date)
        {
            if (typeof(TModel).HasInterface<IAuditEntity>())
                return Repository.DbContext.Set<TModel>()
                    .Where(x => ((IAuditEntity) x).CreatedAt == date)
                    .ToList();

            return new List<TModel>();
        }

        public List<TModel> ReadByCreateDate(DateTime date)
        {
            if (typeof(TModel).HasInterface<IAuditEntity>())
                return Repository.DbContext.Set<TModel>()
                    .Where(x => ((IAuditEntity) x).CreatedAt.Date == date.Date)
                    .ToList();

            return new List<TModel>();
        }
    }
}