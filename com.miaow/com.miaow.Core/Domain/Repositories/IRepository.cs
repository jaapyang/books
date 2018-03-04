using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using com.miaow.Core.Domain.Entities;

namespace com.miaow.Core.Domain.Repositories
{
    public interface IRepository { }

    public interface IRepository<TPrimaryKey, TEntity> : IRepository where TEntity : IEntity<TPrimaryKey>
    {
        IQueryable<TEntity> GetAll();
        Task<IQueryable<TEntity>> GetAllAsync();

        IQueryable<TEntity> Get(Func<TEntity, bool> predicate);
        Task<IQueryable<TEntity>> GetAsync(Func<TEntity, bool> predicate);
        
        TEntity FirstOrDefault(TPrimaryKey id);
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        void Remove(TPrimaryKey id);
        Task RemoveAsync(TPrimaryKey id);
        void Remove(TEntity entity);
        Task RemoveAsync(TEntity entity);

        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);

        void Update(Func<TEntity, bool> filterPredicate, Action<TEntity> updateAction);
        Task UpdateAsync(Func<TEntity, bool> filterPredicate, Action<TEntity> updateAction);

        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity,Task> updateAction);
    }

    public interface IRepository<TEntity>:IRepository<int,TEntity> where TEntity : IEntity<int> { }
}