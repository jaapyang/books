using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.miaow.Core.Extensions;
using com.miaow.DomainService.NovelDomainServices;
using com.miaow.Dtos.NovelDto;
using com.miaow.Models.NovelModel;
using NSoup;

namespace ToolPlat.Handlers
{
    public class NovelListHandler : HandlerBase
    {
        public NovelListHandler(WebBrowser webBrowser) : base(webBrowser)
        {
        }

        public void Load_novel_list(string args)
        {
            List<NovelDto> novelList;

            using (var uow = new NovelUnitOfWork())
            {
                var service = new NovelDomainService(uow);
                novelList = service.GetAll().Select(x => new NovelDto
                {
                    Id = x.Id,
                    MaxChapterIndex = x.MaxChapterIndex,
                    MenuUrl = x.MenuUrl,
                    NovelName = x.NovelName
                }).ToList();
            }

            InvokeScriptFunction("display_novel_list", novelList.ToJson());
        }

        public void Export_Novel(string args)
        {
            ThreadPool.QueueUserWorkItem(state => { Export_Novel_By_Thread(args); });
        }

        public void Export_Novel_By_Thread(string args)
        {
            var novelId = int.Parse(args);

            var dirPath = string.Empty;

            InvokeWindow(() =>
            {
                var dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    dirPath = dialog.SelectedPath;
                }
            });

            if (dirPath.IsNullOrEmpty()) return;

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            NovelModel novelModel;

            using (var uow = new NovelUnitOfWork())
            {
                var service = new NovelDomainService(uow);
                novelModel = service.GetAll().Include(x => x.Chapters).FirstOrDefault(x => x.Id == novelId);
            }

            if (novelModel == null)
            {
                InvokeScriptFunction(() =>
                {
                    Document.InvokeScript("show_message", new[] { $"不存在编号为{novelId}的小说.", "danger" });
                });
                return;
            }

            var filePath = Path.Combine(dirPath, $"{novelModel.NovelName}.txt");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fileStream))
            {
                foreach (var chapterModel in novelModel.Chapters.OrderBy(x => x.SortId).ThenBy(x=>x.Id))
                {
                    sw.WriteLine("\n\n");
                    sw.WriteLine(chapterModel.Title);
                    sw.WriteLine("\n\n");
                    sw.WriteLine(chapterModel.Content.IsNullOrEmpty() ? "该章节缺失" : chapterModel.Content);

                    InvokeScriptFunction(() =>
                    {
                        Document.InvokeScript("show_process",
                                new[] {args, $"正在导出:{chapterModel.Title}", "success"});
                    });
                }
            }

            InvokeScriptFunction(() =>
            {
                Document.InvokeScript("show_process", new[] { args, $"导出完成，请浏览:{filePath}", "success" });
            });
            
        }
        
        public void StartNewPage_Update_Novel(string url)
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
                    string message = $"{downloadCount}/{todoToal} : {chapterModel.Title}";
                    InvokeScriptFunction(() =>
                    {
                        Document.InvokeScript("show_process", new[] { chapterModel.NovelId.ToString(), message, "success" });
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
