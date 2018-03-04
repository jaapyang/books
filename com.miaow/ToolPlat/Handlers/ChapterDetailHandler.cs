using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.miaow.DomainService.NovelDomainServices;
using com.miaow.Dtos.NovelDto;
using com.miaow.Models.NovelModel;

namespace ToolPlat.Handlers
{
    public class ChapterDetailHandler : HandlerBase
    {
        public ChapterDetailHandler(WebBrowser webBrowser) : base(webBrowser)
        {
        }

        public void Load_Chapter_Detail(string args)
        {
            var chapterId = int.Parse(args);
            var chapterInfo = getChapterDetail(chapterId);
            CurrentBrowser.Parent.Text = chapterInfo?.Title;
            var content = chapterInfo?.Content.Replace("\n", "<br/></p><p>");
            InvokeScriptFunction("load_chapter_content", content);
        }

        private ChapterModel getChapterDetail(int chapterId)
        {
            using (var uow = new NovelUnitOfWork())
            {
                var chapterDomainService = new ChapterDomainService(uow);
                return chapterDomainService.FirstOrDefault(chapterId);
            }
        }
    }
}
