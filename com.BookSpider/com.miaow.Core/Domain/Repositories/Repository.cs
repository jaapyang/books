using System;
using System.Collections.Generic;
using System.Linq;
using com.miaow.Core.Domain.Entities;

namespace com.miaow.Core.Domain.Repositories
{
    public abstract class Repository<TPrimaryKey, TEntity> : IRepository<TPrimaryKey, TEntity> where TEntity : class, IEntity<TPrimaryKey>
    {
        public abstract IQueryable<TEntity> GetAll();
        public abstract TEntity FirstOrDefault(Func<TEntity, bool> pridecate);

        public abstract void Add(TEntity entity);
        public abstract void AddRange(IEnumerable<TEntity> list);

        public abstract void Update(TEntity entity);
        public abstract void Update(TPrimaryKey key, Action<TEntity> updateAction);

        public abstract void Delete(TEntity entity);
        public abstract void Delete(Func<TEntity, bool> pridecate);
    }
}