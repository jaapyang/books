namespace com.miaow.Core.Domain.Uow
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Complete();
    }
}