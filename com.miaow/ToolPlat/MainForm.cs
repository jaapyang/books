using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ToolPlat
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class MainForm : Form, IWebBowserForm
    {
        public string CurrentToolName { get; set; }

        public WebBrowser WebBrowser => this._webBrowser;
        
        public MainForm()
        {
            InitializeComponent();

            this.treeView_Tools.Nodes.Add("Tools");

            this.Load += MainForm_Load;
            this.treeView_Tools.AfterSelect += TreeView_Tools_AfterSelect;
            PublicMessage.MessageChanged += PublicMessage_MessageChanged;

            this._webBrowser.ObjectForScripting = this;
            this.treeView_Tools.HideSelection = false;
            ToolMapping.Init();
        }

        private void PublicMessage_MessageChanged(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread t = new Thread(() =>
            {
                this.Text = PublicMessage.Message;
            });
            t.Start();
        }

        private void TreeView_Tools_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.CurrentToolName = e.Node.Text;
            var viewPath = ToolMapping.GetViewPath(this.CurrentToolName);
            this._webBrowser.Url = new Uri(viewPath.ViewPath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var toolName in ToolMapping.GetToolNames())
            {
                this.treeView_Tools.Nodes[0].Nodes.Add(toolName);
            }
            this.treeView_Tools.ExpandAll();
        }

        public void HandlerProcess(string handlerArgsStr)
        {
            try
            {
                var handlerArgs = JsonConvert.DeserializeObject<HandlerArgs>(handlerArgsStr);

                var t = Type.GetType(ToolMapping.GetViewPath(this.CurrentToolName).HandlerFullName);
                var handler =
                    Activator.CreateInstance(t ?? throw new InvalidOperationException($"未找到{CurrentToolName}Handler."),
                        this);
                t.GetMethod(handlerArgs.MethodName)?.Invoke(handler, new[] {handlerArgs.ArgsJsonStr});
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
