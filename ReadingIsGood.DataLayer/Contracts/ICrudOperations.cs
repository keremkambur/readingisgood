using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.DataLayer.Contracts
{
    public interface ICrudOperations<TModel> where TModel : class, IEntity
    {
        TModel Create(TModel entity);
        IEnumerable<TModel> Read(params Expression<Func<TModel, object>>[] includes);
        TModel Read(int id, params Expression<Func<TModel, object>>[] includes);
        TModel Read(Guid uuid, params Expression<Func<TModel, object>>[] includes);
        TModel Update(Guid uuid, Action<TModel> updateCallback);
        Task<TModel> Update(Guid uuid, Func<TModel, Task> updateCallback);
        TModel Delete(int id);
        TModel Delete(Guid uuid);
        TModel Delete(TModel entity);

        List<TModel> ReadByCreateDate(DateTime date);
        List<TModel> ReadByCreateDateTime(DateTime date);


        List<TModel> QueryList(Func<TModel, bool> predicate, int? skip = null, int? take = null,
            params Expression<Func<TModel, object>>[] includes);

        TModel QuerySingle(Func<TModel, bool> predicate, params Expression<Func<TModel, object>>[] includes);
        DbSet<TModel> QueryDbSet();
    }
}