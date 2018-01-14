using System.Linq;
using com.miaow.Core.Domain.Entities;

namespace com.miaow.Core.Domain.Repositories
{
    public interface IRepository { }

    public interface IRepository<TPrimaryKey, TEntity> : IRepository where TEntity : IEntity<TPrimaryKey>
    {
        IQueryable<TEntity> GetAll();
    }

    public interface IRepository<TEntity>:IRepository<int,TEntity> where TEntity : IEntity<int> { }
}