using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using com.BookSpider.DomainService;
using com.BookSpider.Dtos;
using com.BookSpider.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace com.BookSpider.App.Handlers.ConsoleHandlers
{
    class Program
    {
        private static readonly BookDomainService _bookDomainService;
        private static readonly MenuItemDomainService _menuItemDomainService;

        static Program()
        {
            _bookDomainService = new BookDomainService();
            _menuItemDomainService = new MenuItemDomainService();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(@"start....");
            const string hostName = "localhost";
            HandlerRecivedMessage(hostName, "save_chapter", HnadlerChapterMessage);
            //Send_Chapter_update_task();
            //HandlerRecived_BookInfo_Message();

            Console.Read();
        }

        private static void HnadlerChapterMessage(object o, BasicDeliverEventArgs e)
        {
            Console.WriteLine(@"......");
            var josnMessage = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine(josnMessage);
            var chapterInfo = JsonConvert.DeserializeObject<MenuItemInfoDto>(josnMessage);

            var menuItem = _menuItemDomainService.GetAll().FirstOrDefault(x => x.Id == chapterInfo.Id);
            if (menuItem == null || !string.IsNullOrEmpty(chapterInfo.Content)) return;

            _menuItemDomainService.Update(menuItem.Id, x =>
            {
                x.Context = menuItem.Context;
                x.LastUpdateTime = DateTime.Now;
            });
            Console.WriteLine($"成功更新章节:{menuItem.Id}");
        }


        private static void HandlerRecivedMessage(string hostName, string queuename,
            Action<object, BasicDeliverEventArgs> callBack)
        {
            var facotry = new ConnectionFactory { HostName = hostName };

            using (var connection = facotry.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queuename,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (m, e) => callBack(m, e);

                channel.BasicConsume(queue: queuename,
                    autoAck: true,
                    consumer: consumer);
            }
            Console.WriteLine(@"开始信息处理....");
            Console.ReadLine();
        }

        private static void HandlerRecived_BookInfo_Message()
        {
            var facotry = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = facotry.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "book_download",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, e) =>
                {
                    var body = e.Body;
                    var message = Encoding.UTF8.GetString(body);
                    handler_book_message(message);
                };

                channel.BasicConsume(queue: "book_download",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(@"开始接受书籍信息处理....");
                Console.ReadLine();
            }
        }

        public static void Send_Chapter_update_task()
        {
            var menulist = _bookDomainService.GetAll().SelectMany(x => x.MenuList)
                .Select(x=>new MenuItemInfoDto
                {
                    Id = x.Id,
                    Url = x.Url,
                    Content = string.Empty
                }).ToList();
            var jsonMenuList = JsonConvert.SerializeObject(menulist);
            var message = Encoding.UTF8.GetBytes(jsonMenuList);

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "download_chapter",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                //string message = "Hello world! " + $"{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}";

                //var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "download_chapter",
                    basicProperties: null,
                    body: message);
            }
            Console.WriteLine(@"成功发布章节下载任务");
        }
        
        private static void handler_book_message(string message)
        {
            var bookInfoDto = JsonConvert.DeserializeObject<BookInfoDto>(message);

            var bookInfo = new BookInfo()
            {
                BookName = bookInfoDto.BookName,
                MenuUrl = bookInfoDto.MenuUrl,
                MenuList = bookInfoDto.MenuList.Select(x => new MenuItemInfo
                {
                    SortId = x.SortId,
                    Title = x.Title,
                    Url = x.Url
                }).ToList()
            };

            _bookDomainService.Add(bookInfo);
        }
    }
}
