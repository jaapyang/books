using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Core.EntityFramework;
using com.miaow.Models.NovelModel;

namespace com.miaow.DomainService.NovelDomainServices
{
    public class NovelDomainService : EntityFrameworkRepositoryBase<NovelDbContext,NovelModel>
    {
        public NovelDomainService(NovelUnitOfWork uow) : base(uow)
        {
        }

        public NovelModel GetNovelModelById(int id)
        {
            return Get(x => x.Id == id).FirstOrDefault();
        }

        public NovelModel GetNovelModelByUrl(string url)
        {
            return Get(x => x.MenuUrl.Equals(url, StringComparison.CurrentCultureIgnoreCase)).Include(x=>x.Chapters).FirstOrDefault();
        }
        
    }
}
