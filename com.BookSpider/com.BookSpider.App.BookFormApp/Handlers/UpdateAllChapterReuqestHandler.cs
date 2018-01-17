using com.miaow.Core.Queues;

namespace com.BookSpider.App.BookFormApp.Handlers
{
    public class UpdateAllChapterReuqestHandler : SendMessageHandlerBase
    {
        public UpdateAllChapterReuqestHandler()
            : base("download_chapter", "download_chapter")
        {
        }
    }
}