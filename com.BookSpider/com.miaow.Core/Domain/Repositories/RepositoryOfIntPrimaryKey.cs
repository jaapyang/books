using com.miaow.Core.Domain.Entities;

namespace com.miaow.Core.Domain.Repositories
{
    public abstract class Repository<TEntity> : Repository<int, TEntity> where TEntity : class, IEntity<int>
    {
    }
}