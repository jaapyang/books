using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolPlat.Handlers
{
    public sealed class SampleHandler:HandlerBase
    {
        public SampleHandler(IWebBowserForm parentBowserForm) : base(parentBowserForm)
        {
        }
        
        public void ShowMessage(string arg)
        {
            MessageBox.Show(arg);
            InvokeScriptFunction("sayHello","hello paul;");
        }
    }
}
