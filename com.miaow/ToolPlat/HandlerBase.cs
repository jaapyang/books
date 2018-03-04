using System;
using System.Threading;
using System.Windows.Forms;
using com.miaow.Core.Extensions;

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
            CurrentBrowser.DocumentCompleted += CurrentBrowser_DocumentCompleted;
        }

        protected virtual void CurrentBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        protected virtual void InvokeScriptFunction(string functionName, string argsJsonStr)
        {
            CurrentBrowser.Invoke(new Action(() =>
            {
                Document.InvokeScript(functionName, new object[]{ argsJsonStr } );
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