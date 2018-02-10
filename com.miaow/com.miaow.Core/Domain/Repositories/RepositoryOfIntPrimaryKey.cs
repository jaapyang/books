using com.miaow.Core.Domain.Entities;
using com.miaow.Core.Domain.Uow;

namespace com.miaow.Core.Domain.Repositories
{
    public abstract class Repository<TEntity> : Repository<int, TEntity> where TEntity : IEntity<int>
    {
        protected Repository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}