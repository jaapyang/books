using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using NSoup;
using NSoup.Nodes;

namespace ToolPlat.Handlers
{
    public class BiQuGeXiaoShuoHandler : HandlerBase
    {
        public BiQuGeXiaoShuoHandler(IWebBowserForm parentBowserForm) : base(parentBowserForm)
        {
        }

        public void Parse_Menu_Url(string url)
        {
            var domainUrl = "http://www.biquge.com.tw";
            var connect = NSoupClient.Connect(url);
            var document = connect.Get();
            var linkArray = document?.GetAllElements().Where(x => x.TagName() == "a").ToList();

            var pattern = @"/\d+_\d+/\d+.html";

            var arrayList = new ArrayList();

            foreach (Element element in linkArray)
            {
                var href = element.Attr("href");
                if (!element.HasText || !Regex.IsMatch(href, pattern)) continue;

                var text = element.Text();

                arrayList.Add(new
                {
                    Url = $"{domainUrl}{href}",
                    Title = text
                });
            }

            var argsStr = JsonConvert.SerializeObject(arrayList);

            InvokeScriptFunction("loadMenu", argsStr);
        }
    }
}
