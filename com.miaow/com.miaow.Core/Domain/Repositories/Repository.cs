using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public virtual Task<IQueryable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public abstract IQueryable<TEntity> Get(Func<TEntity, bool> predicate);

        public virtual Task<IQueryable<TEntity>> GetAsync(Func<TEntity, bool> predicate)
        {
            return Task.FromResult(GetAll().Where(x => predicate(x)));
        }

        public virtual TEntity FirstOrDefault(TPrimaryKey id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return Task.FromResult(FirstOrDefault(id));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public abstract void Remove(TPrimaryKey id);

        public virtual Task RemoveAsync(TPrimaryKey id)
        {
            Remove(id);
            return Task.FromResult(0);
        }

        public abstract void Remove(TEntity entity);

        public virtual Task RemoveAsync(TEntity entity)
        {
            Remove(entity);
            return Task.FromResult(0);
        }

        public abstract TEntity Add(TEntity entity);

        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
            return Task.FromResult(Add(entity));
        }

        public abstract void Update(Func<TEntity, bool> filterPredicate, Action<TEntity> updateAction);

        public virtual Task UpdateAsync(Func<TEntity, bool> filterPredicate, Action<TEntity> updateAction)
        {
            Update(filterPredicate, updateAction);
            return Task.FromResult(0);
        }

        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = FirstOrDefault(id);
            updateAction(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity,Task> updateAction)
        {
            var entity = await FirstOrDefaultAsync(id);
            await updateAction(entity);
            return entity;
        }
        
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}