using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.miaow.Tests.TestModels;
using EasyNetQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace com.miaow.Tests.Queues.SubscriberApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                receive_message();
            }
            catch (Exception e)
            {
                using (var fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.log"), FileMode.Append, FileAccess.Write))
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine(e.Message);
                }
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }

        private static void receive_message()
        {
            string QueueName = "com.miaow.queues.test";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                Console.WriteLine(" [x] waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (o, e) =>
                {
                    handle_TextMessage(o, e);
                    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

                Console.WriteLine(" [x] Press [Enter] to exit.");
                Console.ReadLine();
            }
        }

        private static void handle_TextMessage(object o, BasicDeliverEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Got message: {0}", Encoding.UTF8.GetString(e.Body));
            Console.ResetColor();
        }
    }
}
