using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.BookSpider.Model;
using com.miaow.Core.EfRepository;

namespace com.BookSpider.DomainService
{
    public class BookDbContext : EfDbContext
    {
        public BookDbContext() : base("com.BookSpider.DbConnectionString")
        {
        }

        public DbSet<BookInfo> BookInfos { get; set; }
    }
}
