using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.BookSpider.Dtos;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace com.BookSpider.App.Handlers.ConsoleHandlers
{
    class Program
    {
        static void Main(string[] args)
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

                Console.WriteLine("Press [Entern] to exit.");
                Console.ReadLine();
            }
        }


        private static void handler_book_message(string message)
        {
            var bookInfo = JsonConvert.DeserializeObject<BookInfoDto>(message);

            foreach (var menuItemInfo in bookInfo.MenuList.OrderBy(x => x.SortId))
            {
                Console.WriteLine(menuItemInfo.Title);
            }
        }
    }
}
