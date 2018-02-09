using System;
using System.Linq;
using com.miaow.Core.Domain.Entities;
using com.miaow.Core.Domain.Uow;

namespace com.miaow.Core.Domain.Repositories
{
    public abstract class Repository<TPrimaryKey, TEntity> : IRepository<TPrimaryKey, TEntity> where TEntity : IEntity<TPrimaryKey>
    {
        protected Repository(IUnitOfWork uow)
        {
            
        }

        public abstract IQueryable<TEntity> GetAll();
        public abstract IQueryable<TEntity> Get(Func<TEntity, bool> predicate);
        public abstract TEntity GetById(TPrimaryKey id);
        public abstract void Remove(TEntity entity);

        public abstract TEntity Add(TEntity entity);
        public abstract void Update(Func<TEntity, bool> filterPredicate, Action<TEntity> updateAction);

        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = GetById(id);
            updateAction(entity);
            return entity;
        }
    }
}