using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ReadingIsGood.EntityLayer.Database.Base;
using ReadingIsGood.EntityLayer.Database.Content;

namespace ReadingIsGood.DataLayer.Contracts
{
    public interface IDatabaseRepository : IDisposable
    {
        ICrudOperations<Product> ProductCrudOperations { get; }
        ICrudOperations<Order> OrderCrudOperations { get; }
        ICrudOperations<OrderDetail> OrderDetailCrudOperations { get; }

        bool BulkMode { get; }

        void EnableBulkMode();

        void DisableBulkMode(bool saveChanges = false);

        int CommitChanges();

        Task<int> CommitChangesAsync();

        Task LoadReference<TEntry, TProperty>(TEntry entity, Expression<Func<TEntry, TProperty>> expression,
            CancellationToken cancellationToken)
            where TEntry : class, IEntity
            where TProperty : class;

        Task LoadCollection<TEntry, TProperty>(TEntry entity,
            Expression<Func<TEntry, IEnumerable<TProperty>>> expression, CancellationToken cancellationToken)
            where TEntry : class, IEntity
            where TProperty : class;

        /// <summary>
        ///     detaches an entity from the context. The read will fetch it from the data source again.
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="entity"></param>
        void Detach<TEntry>(TEntry entity);

        /// <summary>
        ///     reload the entity from the data source
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntry> Reload<TEntry>(TEntry entity);

        bool Any<TEntity>() where TEntity : class, IEntity;

        bool Any<TEntity>(Func<TEntity, bool> predicate) where TEntity : class, IEntity;

        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class, IEntity;

        TEntity QueryEntity<TEntity>(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

        IQueryable<TEntity> QueryEntities<TEntity>(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity;

        IEnumerable<TEntity> GetEntities<TEntity>(int pageSize = 0, int pageNumber = 0,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity;

        void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity;
    }
}