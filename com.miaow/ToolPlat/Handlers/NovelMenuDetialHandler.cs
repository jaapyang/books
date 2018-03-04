using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.miaow.Core.Extensions;
using com.miaow.DomainService.NovelDomainServices;
using com.miaow.Dtos.NovelDto;

namespace ToolPlat.Handlers
{
    public class NovelMenuDetialHandler:HandlerBase
    {
        public NovelMenuDetialHandler(WebBrowser webBrowser) : base(webBrowser)
        {
        }

        public void Show_Menu_Detail(string args)
        {
            var novelId = int.Parse(args);
            var chapterList = GetValue(novelId);
            InvokeScriptFunction("show_hello", chapterList.ToJson());
        }

        private List<ChapterDto> GetValue(int novelId)
        {
            using (var uow = new NovelUnitOfWork())
            {
                var novelService = new ChapterDomainService(uow);
                return novelService.GetAll().Where(x => x.NovelId == novelId).OrderBy(x => x.SortId)
                    .Select(x => new ChapterDto
                    {
                        Id = x.Id,
                        NovelId = x.NovelId,
                        SortId = x.SortId,
                        Title = x.Title,
                        Url = x.Url
                    }).ToList();
            }
        }
        
    }
}
