using System.Linq;
using com.miaow.Core.Domain.Entities;

namespace com.miaow.Core.Domain.Repositories
{
    public abstract class Repository<TPrimaryKey, TEntity> : IRepository<TPrimaryKey, TEntity> where TEntity : IEntity<TPrimaryKey>
    {
        public abstract IQueryable<TEntity> GetAll();
    }
}