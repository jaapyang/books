using System.Data.Entity;
using com.miaow.Core.Domain;

namespace com.miaow.Core.EfRepository
{
    public class EfDbContext : DbContext, IDbContext
    {
        public EfDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }
}