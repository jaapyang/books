using System;
using System.Collections.Generic;
using System.Linq;
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

        static Program()
        {
            _bookDomainService = new BookDomainService();
        }

        static void Main(string[] args)
        {
            Send_Chapter_update_task();
            HandlerRecivedMessage();
        }

        private static void HandlerRecivedMessage()
        {
            var facotry = new ConnectionFactory() {HostName = "localhost"};

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

                Console.WriteLine("Press [Entern] to exit.");
                Console.ReadLine();
            }
        }

        public static void Send_Chapter_update_task()
        {
            // TODO: sent download task to Python
            var chapterInfoList = _bookDomainService.GetAll().SelectMany(x => x.MenuList).ToList();

            foreach (var menuItemInfo in chapterInfoList)
            {
                Console.WriteLine($"{menuItemInfo.Title}\t{menuItemInfo.Url}");
            }
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
