using System;
using System.Collections.Generic;
using System.Linq;
using com.miaow.Core.Domain.Entities;

namespace com.miaow.Core.Domain.Repositories
{
    public interface IRepository { }

    public interface IRepository<TPrimaryKey, TEntity> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        IQueryable<TEntity> GetAll();
        TEntity FirstOrDefault(Func<TEntity, bool> pridecate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> list);

        void Update(TEntity entity);
        void Update(Func<TEntity, bool> filterPridecate, Action<TEntity> updateAction);

        void Delete(TEntity entity);
        void Delete(Func<TEntity, bool> pridecate);
    }

    public interface IRepository<TEntity> : IRepository<int, TEntity> where TEntity : class, IEntity<int> { }
}