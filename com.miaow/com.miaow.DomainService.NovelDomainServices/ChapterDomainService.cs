using System;
using System.Collections.Generic;
using System.Linq;
using com.miaow.Core.EntityFramework;
using com.miaow.Models.NovelModel;

namespace com.miaow.DomainService.NovelDomainServices
{
    public class ChapterDomainService: EntityFrameworkRepositoryBase<NovelDbContext,ChapterModel>
    {
        public ChapterDomainService(EfUnitOfWork uow) : base(uow)
        {
        }

        public void UpdateChapterList(List<ChapterModel> chapterList)
        {
            foreach (var chapter in chapterList)
            {
                Update(chapter.Id, x =>
                {
                    x.Content = chapter.Content;
                    x.Url = chapter.Url;
                    x.LastUpdatedTime = chapter.LastUpdatedTime;
                });
            }
        }
    }
}