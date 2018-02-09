using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Core.Domain.Uow;

namespace com.miaow.Core.EntityFramework
{
    public abstract class EfUnitOfWork : IUnitOfWork,IDisposable
    {
        private Dictionary<string, DbContext> _activeDbContexts;
        private List<DbTransaction> _transactionList;

        public EfUnitOfWork()
        {
            _activeDbContexts = new Dictionary<string, DbContext>();
            _transactionList = new List<DbTransaction>();
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext:DbContext
        {
            var typeName = typeof(TDbContext).Name;
            if (_activeDbContexts.Any(x => x.Key == typeName))
            {
                return _activeDbContexts[typeName] as TDbContext;
            }

            var dbContext = Activator.CreateInstance<TDbContext>();
            _activeDbContexts.Add(typeName,dbContext);
            return dbContext;
        }

        public virtual void BeginTransaction()
        {
            foreach (var dbContext in _activeDbContexts.Values)
            {
                _transactionList.Add(dbContext.Database.Connection.BeginTransaction());
            }
        }

        public virtual void Complete()
        {
            try
            {
                foreach (var dbContext in _activeDbContexts.Values)
                {
                    dbContext.SaveChanges();
                }

                foreach (var dbTransaction in _transactionList)
                {
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                foreach (var dbTransaction in _transactionList)
                {
                    dbTransaction.Rollback();
                }
                throw ex;
            }
        }

        public void Dispose()
        {
            Complete();
        }
    }
}
