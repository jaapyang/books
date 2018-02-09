using System;
using System.Linq;
using com.miaow.Core.Domain.Entities;

namespace com.miaow.Core.Domain.Repositories
{
    public interface IRepository { }

    public interface IRepository<TPrimaryKey, TEntity> : IRepository where TEntity : IEntity<TPrimaryKey>
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Func<TEntity, bool> predicate);
        TEntity GetById(TPrimaryKey id);
        void Remove(TEntity entity);
        TEntity Add(TEntity entity);
        void Update(Func<TEntity, bool> filterPredicate, Action<TEntity> updateAction);
        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);
    }

    public interface IRepository<TEntity>:IRepository<int,TEntity> where TEntity : IEntity<int> { }
}