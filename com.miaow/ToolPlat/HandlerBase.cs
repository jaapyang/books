using System;
using System.Threading;
using System.Windows.Forms;

namespace ToolPlat
{
    public abstract class HandlerBase
    {
        //public IWebBowserForm ParentForm { get; private set; }
        protected WebBrowser CurrentBrowser { get; private set; }
        public HtmlDocument Document { get; }

        public HandlerBase(WebBrowser webBrowser)
        {
            CurrentBrowser = webBrowser;
            this.Document = CurrentBrowser.Document;
        }
        
        protected virtual void InvokeScriptFunction(string functionName, params object[] argsJsonStr)
        {
            CurrentBrowser.Invoke(new Action(() =>
            {
                Document.InvokeScript(functionName, argsJsonStr);
            }));
        }

        protected virtual void InvokeScriptFunction(Action action)
        {
            CurrentBrowser.Invoke(action);
        }

        protected virtual void InvokeWindow(Action action)
        {
            CurrentBrowser.FindForm()?.Invoke(action);
        }
    }
}