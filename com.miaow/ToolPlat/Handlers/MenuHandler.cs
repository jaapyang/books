using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ToolPlat.Handlers
{
    public class MenuHandler:HandlerBase
    {
        public MenuHandler(IWebBowserForm parentBowserForm) : base(parentBowserForm)
        {
        }

        public void Parse_Menu_Url(string url)
        {
            var array = new ArrayList();

            for (int i = 0; i < 1000; i++)
            {
                array.Add(new
                {
                    Url = "Http://www.baidu.com",
                    Title = $"www.baidu_{i}.com"
                });
            }

            var argsStr = JsonConvert.SerializeObject(array);

            InvokeScriptFunction("loadMenu",argsStr);
        }
    }
}
