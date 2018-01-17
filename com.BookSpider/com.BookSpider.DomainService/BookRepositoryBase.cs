using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Core.Domain.Entities;
using com.miaow.Core.EfRepository;

namespace com.BookSpider.DomainService
{
    public abstract class BookRepositoryBase<TPrimaryKey,TEntity>:EfRepository<TPrimaryKey,TEntity>
        where TEntity:class,IEntity<TPrimaryKey>
    {
        protected BookRepositoryBase() : base(new BookDbContext())
        {
        }
    }
}
