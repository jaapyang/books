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
    public partial class MainForm : Form
    {
        public string CurrentToolName { get; set; }

        public MainForm()
        {
            InitializeComponent();

            this.treeView_Tools.Nodes.Add("Tools");

            #region Events

            this.Load += MainForm_Load;
            //this.treeView_Tools.AfterSelect += TreeView_Tools_AfterSelect;

            #endregion

            this._webBrowser.ObjectForScripting = this;
            this.treeView_Tools.HideSelection = false;
            ToolMapping.Init();
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
            HandlerProcess(handlerArgsStr, this.tabControl1.SelectedTab.Controls[0] as WebBrowser);
        }

        public void HandlerProcess(string handlerArgsStr, WebBrowser currentWebBrowser)
        {
            try
            {
                var requestMethodArgs = JsonConvert.DeserializeObject<RequestMethodArgs>(handlerArgsStr);

                var t = Type.GetType(ToolMapping.GetViewPath(this.CurrentToolName).HandlerFullName);
                var handler =
                    Activator.CreateInstance(t ?? throw new InvalidOperationException($"未找到{CurrentToolName}Handler."),
                        currentWebBrowser);
                t.GetMethod(requestMethodArgs.MethodName)?.Invoke(handler, new[] { requestMethodArgs.ArgsJsonStr });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem_openInNewTab_Click(object sender, EventArgs e)
        {
            this.CurrentToolName = this.treeView_Tools.SelectedNode.Text;

            AppendTabPageForTool(CurrentToolName);
        }

        public void AppendTabPageForTool(string currentToolName)
        {
            var viewPath = ToolMapping.GetViewPath(currentToolName);
            var webBrowser = new WebBrowser
            {
                ObjectForScripting = this,
                Dock = DockStyle.Fill,
                Url = new Uri(viewPath.ViewPath)
            };
            
            var tabPage = new TabPage(CurrentToolName);
            tabPage.Controls.Add(webBrowser);

            this.tabControl1.TabPages.Add(tabPage);
            this.tabControl1.SelectTab(tabPage);
        }


        private void treeView_Tools_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            this.CurrentToolName = e.Node.Text;
            this.tabControl1.SelectedTab.Text = CurrentToolName;
            var viewPath = ToolMapping.GetViewPath(this.CurrentToolName);
            if (!(this.tabControl1.SelectedTab.Controls[0] is WebBrowser activeWebBrowser)) return;
            activeWebBrowser.Url = new Uri(viewPath.ViewPath);
        }
    }
}
