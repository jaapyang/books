using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.BookSpider.App.BookFormApp.Handlers;
using com.BookSpider.DomainService;
using com.BookSpider.Dtos;
using RabbitMQ.Client;

namespace com.BookSpider.App.BookFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.textBox1.MaxLength = int.MaxValue;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //todo:实现全部更新功能 
        }

        private void 提交解析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var html = this.textBox1.Text;
            MessageBox.Show(html.Length.ToString());

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "parse_menu_html",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                //string message = "Hello world! " + $"{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}";

                var body = Encoding.UTF8.GetBytes(html);

                channel.BasicPublish(exchange: "",
                    routingKey: "parse_menu_html",
                    basicProperties: null,
                    body: body);
            }
            MessageBox.Show("成功发布章节解析任务");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var service = new MenuItemDomainService();
            var menuitemList = service.GetAll().Where(x => x.Context == null || x.Context.Length < 10)
                .Select(x => new MenuItemInfoDto
                {
                    Id = x.Id,
                    Url = x.Url,
                    Content = string.Empty
                }).ToList();

            var handler = new UpdateAllChapterReuqestHandler();
            handler.SendMessageProcesser(menuitemList);
        }
    }
}
