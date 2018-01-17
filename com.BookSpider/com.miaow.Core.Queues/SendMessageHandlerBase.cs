using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace com.miaow.Core.Queues
{
    public abstract class SendMessageHandlerBase : IQueueHandler
    {
        public string HostName { get; set; } = "localhost";
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }

        public string Exchange { get; set; } = "";


        private SendMessageHandlerBase() { }
        protected SendMessageHandlerBase(string queueName, string routingKey)
            : this("localhost", queueName, routingKey)
        { }

        protected SendMessageHandlerBase(string hostName, string queueName, string routingKey) : this()
        {
            HostName = hostName;
            QueueName = queueName;
            RoutingKey = routingKey;
        }

        public virtual void SendMessageProcesser(object messageBody)
        {
            var json_message = JsonConvert.SerializeObject(messageBody);
            var message = Encoding.UTF8.GetBytes(json_message);

            var factory = new ConnectionFactory() { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.BasicPublish(exchange: Exchange,
                    routingKey: RoutingKey,
                    basicProperties: null,
                    body: message);
            }
        }
    }
}