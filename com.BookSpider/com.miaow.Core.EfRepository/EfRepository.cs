using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Core.Domain.Entities;
using com.miaow.Core.Domain.Repositories;

namespace com.miaow.Core.EfRepository
{
    public class EfRepository<TPrimaryKey, TEntity> : Repository<TPrimaryKey, TEntity>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly EfDbContext dbContext;
        protected virtual DbSet<TEntity> Table { get; private set; }

        protected EfRepository(EfDbContext context)
        {
            dbContext = context;
            Table = dbContext.Set<TEntity>();
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Table.AsQueryable();
        }

        public override TEntity FirstOrDefault(Func<TEntity, bool> pridecate)
        {
            return Table.FirstOrDefault(pridecate);
        }

        public override void Add(TEntity entity)
        {
            Table.Add(entity);
            dbContext.SaveChanges();
        }

        public override void AddRange(IEnumerable<TEntity> list)
        {
            Table.AddRange(list);
            dbContext.SaveChanges();
        }

        public override void Update(TEntity entity)
        {
            
        }

        public override void Update(Func<TEntity,bool> filterPridecate, Action<TEntity> updateAction)
        {
            var item = Table.FirstOrDefault(filterPridecate);
            if (item == null) return;
            updateAction(item);
            dbContext.SaveChanges();
        }

        public override void Delete(TEntity entity)
        {
            Table.Remove(entity);
            dbContext.SaveChanges();
        }

        public override void Delete(Func<TEntity, bool> pridecate)
        {
            var entity = FirstOrDefault(pridecate);
            Table.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
