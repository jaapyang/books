using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Tests.TestModels;
using EasyNetQ;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace com.miaow.Tests.Queues.PublisherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            string message = String.Empty;

            while ((message = Console.ReadLine()) != "q")
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "com.miaow.queues.test",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    Console.ForegroundColor = ConsoleColor.Red;
                    var textMessage = new TextMessage()
                    {
                        Text = message
                    };
                    var jsonMessage = JsonConvert.SerializeObject(textMessage);
                    var data = Encoding.UTF8.GetBytes(jsonMessage);

                    channel.BasicPublish(exchange: "",
                        routingKey: "com.miaow.queues.test",
                        basicProperties: null,
                        body: data);

                    Console.WriteLine("had send message: {0}.", message);
                    Console.ResetColor();
                }
            }
            Console.ReadLine();
        }
    }
}
