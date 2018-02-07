namespace com.miaow.Core.Domain.Uow
{
    public interface IUnitOfWork
    {
        void SaveChanges();
    }
}