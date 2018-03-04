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
using NSoup.Nodes;

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
                novelList = Task.FromResult(service.GetAll().OrderByDescending(x => x.Id).Select(x => new NovelDto
                {
                    Id = x.Id,
                    MaxChapterIndex = x.MaxChapterIndex,
                    MenuUrl = x.MenuUrl,
                    NovelName = x.NovelName
                })).Result.ToList();
            }

            InvokeScriptFunction("display_novel_list", novelList.ToJson());
        }

        public void Export_Novel(string args)
        {
            ThreadPool.QueueUserWorkItem(state => { Export_Novel_By_Thread(args); });
        }

        public void Export_novel_last_chapters(string args)
        {
            ThreadPool.QueueUserWorkItem(state => { Export_Novel_By_Thread(args, 10); });
        }

        public void Export_Novel_By_Thread(string args,int exportCount = 0)
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

            var exportQuery = novelModel.Chapters.OrderBy(x => x.SortId).ThenBy(x => x.Id).AsEnumerable();
            if (exportCount > 0)
            {
                var today = DateTime.Now;
                filePath = Path.Combine(dirPath, $"{novelModel.NovelName}_{today.Year}{today.Month}{today.Day}.txt");
                exportQuery = exportQuery.Skip(novelModel.Chapters.Count - exportCount);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fileStream,Encoding.UTF8))
            {
                
                foreach (var chapterModel in exportQuery.ToList())
                {
                    sw.WriteLine("\n\n");
                    sw.WriteLine(chapterModel.Title);
                    sw.WriteLine("\n\n");
                    sw.WriteLine(chapterModel.Content.IsNullOrEmpty() ? "该章节缺失" : chapterModel.Content);

                    InvokeScriptFunction(() =>
                    {
                        Document.InvokeScript("show_process",
                                new[] { args, $"正在导出:{chapterModel.Title}", "success" });
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

                InvokeScriptFunction(() =>
                {
                    Document.InvokeScript("show_process", new[] { novelModel.Id.ToString(), "更新完成.", "success" });
                });
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

        public void Update_all_novel_chapters(string args)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Update_all_novel_chapters_byThread(args);
            });
        }

        public void Update_all_novel_chapters_byThread(string args)
        {
            using (var uow = new NovelUnitOfWork())
            {
                var novelService = new NovelDomainService(uow);

                var novelList = novelService.GetAll().Include(x => x.Chapters).ToList();

                InvokeScriptFunction("display_novel_list", novelList.OrderByDescending(x=>x.Id).Select(x => new NovelDto
                {
                    Id = x.Id,
                    MaxChapterIndex = x.MaxChapterIndex,
                    MenuUrl = x.MenuUrl,
                    NovelName = x.NovelName
                }).ToList().ToJson());
                
                foreach (var novelModel in novelList)
                {
                    if (novelModel.Chapters.IsNullOrEmpty()) novelModel.Chapters = new List<ChapterModel>();

                    novelModel.LastUpdateTime = DateTime.Now;
                    var chapterDtoList = Get_ChapterList(novelModel);

                    if (chapterDtoList.IsNullOrEmpty())
                    {
                        InvokeScriptFunction(() =>
                        {
                            Document.InvokeScript(
                                "show_process",
                                new[] { novelModel.Id.ToString(), "暂无更新", "success" });
                        });
                        continue;
                    }
                    
                    novelModel.Chapters.AddRange(chapterDtoList.Select(x => new ChapterModel
                    {
                        LastUpdatedTime = DateTime.Now,
                        SortId = x.SortId,
                        Title = x.Title,
                        Url = x.Url
                    }));

                    novelModel.MaxChapterIndex = novelModel.Chapters.Max(x=>x.SortId);

                    InvokeScriptFunction(() =>
                    {
                        Document.InvokeScript(
                            "show_process",
                            new[] {novelModel.Id.ToString(), $"章节更新完成.新增{chapterDtoList.Count}章", "success"});
                    });
                }
            }
        }
        
        private List<ChapterDto> Get_ChapterList(NovelModel novelModel)
        {
            Document document;
            Download_PageContent(novelModel, 0, out document);

            if (document == null) return null;

            var domainPattern = @"^(?<domain>http://\w+.*?)/";
            var domainUrl = Regex.Match(novelModel.MenuUrl, domainPattern).Groups["domain"].Value;

            var linkArray = document.GetAllElements().Where(x => x.TagName() == "a").ToList();
            var pattern = @"(?<novelId>.*/)(?<chapterId>.*html$)";

            var chapterDtoList = new List<ChapterDto>();
            var chapterSortId = novelModel.MaxChapterIndex;
            foreach (var element in linkArray)
            {
                var href = element.Attr("href");
                if (!element.HasText || !Regex.IsMatch(href, pattern)) continue;
                
                var chapterUrl = $"{domainUrl}{href}";
                var text = element.Text();

                if (novelModel.Chapters.Any(x => x.Url.Equals(chapterUrl, StringComparison.CurrentCultureIgnoreCase))) continue;

                chapterSortId++;
                chapterDtoList.Add(new ChapterDto
                {
                    Title = text,
                    Url = chapterUrl,
                    SortId = chapterSortId
                });
            }

            return chapterDtoList;
        }

        private void Download_PageContent(NovelModel novelModel,int retryCount,out Document document)
        {
            try
            {
                var connect = NSoupClient.Connect(novelModel.MenuUrl);
                document = connect.Get();
            }
            catch (Exception ex)
            {
                if (retryCount > 5)
                {
                    document = null;
                    return;
                }

                Thread.Sleep(3 * 1000);
                Download_PageContent(novelModel, ++retryCount,out document);
                InvokeScriptFunction(() => { Document.InvokeScript("show_message", new[] {ex.Message, "danger"}); });
            }
        }
        
    }
}
