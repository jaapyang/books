using System.Data.Entity;
using com.miaow.Core.Domain;
using com.miaow.Models.NovelModel;

namespace com.miaow.DomainService.NovelDomainServices
{
    public class NovelDbContext:DbContext
    {
        public NovelDbContext() : base("NovelConnectionString")
        {
            
        }

        public DbSet<NovelModel> NovelModels { get; set; }

        public DbSet<ChapterModel> ChapterModels { get; set; }
        
    }
}