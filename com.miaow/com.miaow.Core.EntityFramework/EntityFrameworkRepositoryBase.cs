using System;
using System.Data.Entity;
using System.Linq;
using com.miaow.Core.Domain.Entities;
using com.miaow.Core.Domain.Repositories;

namespace com.miaow.Core.EntityFramework
{
    public abstract class EntityFrameworkRepositoryBase<TDbContext, TEntity> 
        : EntityFrameworkRepositoryBase<TDbContext, int, TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        protected EntityFrameworkRepositoryBase(EfUnitOfWork uow) : base(uow)
        {
        }
    }

    public abstract class EntityFrameworkRepositoryBase<TDbContext, TPrimaryKey, TEntity>
        : Repository<TPrimaryKey,TEntity>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
    {
        private readonly DbContext _dbContext;

        protected EntityFrameworkRepositoryBase(EfUnitOfWork uow) : base(uow)
        {
            _dbContext = uow.GetOrCreateDbContext<TDbContext>();
        }
        
        public override IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsQueryable();
        }

        public override IQueryable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate).AsQueryable();
        }
        
        public override TEntity Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            return entity;
        }

        public override void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public override void Remove(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null) return;
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public override void Update(Func<TEntity, bool> filterPredicate, Action<TEntity> updateAction)
        {
            _dbContext.Set<TEntity>().Where(filterPredicate).AsQueryable().ForEachAsync(updateAction);
        }
    }
}