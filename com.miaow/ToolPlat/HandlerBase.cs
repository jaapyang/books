using System.Threading;
using System.Windows.Forms;

namespace ToolPlat
{
    public abstract class HandlerBase
    {
        public IWebBowserForm ParentForm { get; private set; }

        public HtmlDocument Document { get; }

        public HandlerBase(IWebBowserForm parentBowserForm)
        {
            ParentForm = parentBowserForm;
            this.Document = parentBowserForm.WebBrowser.Document;
        }

        protected virtual void InvokeScriptFunction(string functionName, string argsJsonStr)
        {
            Document.InvokeScript(functionName, new[] {argsJsonStr});
        }
    }

    public interface IWebBowserForm
    {
        WebBrowser WebBrowser { get; }
    }
}