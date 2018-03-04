using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.miaow.Core;
using com.miaow.DomainService.NovelDomainServices;
using com.miaow.Models.NovelModel;
using Newtonsoft.Json;
using NSoup;
using NSoup.Nodes;

namespace ToolPlat.Handlers
{
    public class BiQuGeXiaoShuoHandler : HandlerBase
    {
        public BiQuGeXiaoShuoHandler(WebBrowser webBrowser) : base(webBrowser)
        {
        }

        public void Parse_Menu_Url(string url)
        {
            var domainUrl = "http://www.biquge.com.tw";

            Document document = null;
            try
            {
                var connect = NSoupClient.Connect(url);
                document = connect.Get();
            }
            catch (Exception ex)
            {
                InvokeScriptFunction(() =>
                {
                    Document.InvokeScript("show_message", new[] {ex.Message, "danger"});
                });
                return;
            }

            var linkArray = document?.GetAllElements().Where(x => x.TagName() == "a").ToList();
            
            var novelName = document?.GetElementById("info").Children.First.Text();

            CurrentBrowser.Parent.Text = novelName;

            using (var uow = new NovelUnitOfWork())
            {
                uow.BeginTransaction();

                var service = new NovelDomainService(uow);
                var novelModel = service.GetNovelModelByUrl(url);
                var isUpdate = novelModel != null;
                if (novelModel == null)
                {
                    novelModel = new NovelModel {Chapters = new List<ChapterModel>()};
                }
                
                novelModel.MenuUrl = url;
                novelModel.NovelName = novelName;
                novelModel.LastUpdateTime = DateTime.Now;

                int chapterCount = 0;
                var pattern = @"/\d+_\d+/\d+.html";

                foreach (Element element in linkArray)
                {
                    var href = element.Attr("href");
                    if (!element.HasText || !Regex.IsMatch(href, pattern)) continue;

                    chapterCount++;

                    var chapterUrl = $"{domainUrl}{href}";
                    var text = element.Text();

                    if (novelModel.Chapters.Any(x => x.Url.Equals(chapterUrl, StringComparison.CurrentCultureIgnoreCase))) continue;
                    
                    novelModel.Chapters.Add(new ChapterModel
                    {
                        Title = text,
                        Url = chapterUrl,
                        SortId = chapterCount,
                        LastUpdatedTime = DateTime.Now
                    });
                }

                novelModel.MaxChapterIndex = chapterCount;

                AppManagement.Cache.Set(url, novelModel);

                if (!novelModel.Chapters.Any())
                {
                    InvokeScriptFunction(() =>
                    {
                        Document.InvokeScript("show_message", new[] { "没有解析出任何内容", "danger" });
                    });
                }

                var argsStr = JsonConvert.SerializeObject(novelModel.Chapters.Where(x => x.Id <= 0)
                    .OrderBy(x => x.SortId).Select(x => new
                    {
                        x.Url,
                        x.Title
                    }).ToList());

                InvokeScriptFunction("loadMenu", argsStr);

                if (isUpdate) return;

                service.Add(novelModel);
            }
        }

        public void Parse_All(string url)
        {
            Thread t = new Thread(() =>
            {
                NovelModel novelModel;

                using (var uow = new NovelUnitOfWork())
                {
                    var service = new NovelDomainService(uow);
                    novelModel = service.GetNovelModelByUrl(url);
                }

                if (novelModel == null) return;

                var todoToal = novelModel.Chapters.Count(x => string.IsNullOrEmpty(x.Content));

                var downloadCount = 0;

                foreach (var chapterModel in novelModel.Chapters.Where(x => string.IsNullOrEmpty(x.Content)))
                {
                    Parse_Chapter(chapterModel);
                    downloadCount++;
                    string message = $"当前进度:{downloadCount}/{todoToal};正在下载 {chapterModel.Title}";
                    InvokeScriptFunction(() =>
                    {
                        Document.InvokeScript("show_message", new[] { message, "success"});
                    });
                }
            });
            t.Start();
        }

        public void Parse_Chapter(ChapterModel chapterModel)
        {
            Random r = new Random();
            var delayTimeSpance = r.Next(1, 10);
            Thread.Sleep(delayTimeSpance * 1000);

            try
            {
                var connection = NSoupClient.Connect(chapterModel.Url);
                var document = connection.Get();
                var element = document.GetElementById("content");
                var contentHtml = element.Html();
                var lines = Regex.Replace(contentHtml, "<br.*?/>", "");
                lines = Regex.Replace(lines, "&nbsp;", " ");

                if (lines.Length <= 0) return;

                using (var uow = new NovelUnitOfWork())
                {
                    var chapterService = new ChapterDomainService(uow);
                    chapterService.Update(chapterModel.Id, x =>
                    {
                        x.Content = lines;
                        x.LastUpdatedTime = DateTime.Now;
                    });
                }
            }
            catch (Exception e)
            {
                //InvokeScriptFunction("show_message", e.Message);

                InvokeScriptFunction(() =>
                {
                    Document.InvokeScript("show_message", new[] { e.Message, "danger" });
                });
            }

        }
    }
}
