using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace com.miaow.Core.Queues
{
    public abstract class ReceivedMessageHandlerBase : IQueueHandler
    {
        private ReceivedMessageHandlerBase() { }

        protected ReceivedMessageHandlerBase(string queueName)
            : this("localhost", queueName){}

        protected ReceivedMessageHandlerBase(string hostName, string queueName)
        {
            this.HostName = hostName;
            this.QueueName = queueName;
        }

        public string HostName { get; set; } = "localhost";
        public string QueueName { get; set; }
        public virtual bool Durable { get; set; } = true;
        public virtual bool Exclusive { get; set; } = true;
        public virtual bool AutoDelete { get; set; } = false;
        public virtual bool Global { get; set; } = false;
        public virtual ushort PrefetchCount { get; set; } = 1;
        public virtual uint PrefetchSize { get; set; } = 0;
        public virtual IDictionary<string, object> Arguments { get; set; } = null;

        public void ReciveMessageProcesser()
        {
            var factory = new ConnectionFactory() { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName,
                    durable: Durable,
                    exclusive: Exclusive,
                    autoDelete: AutoDelete,
                    arguments: Arguments);
                channel.BasicQos(prefetchSize: PrefetchSize, prefetchCount: PrefetchCount, global: Global);
                Console.WriteLine(" [x] waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (o, e) =>
               {
                   HandlerMessage(o, e);
                   channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
               };

                channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

                Console.WriteLine(" [x] Press [Enter] to exit.");
                Console.ReadLine();
            }
        }

        protected abstract void HandlerMessage(object sender, BasicDeliverEventArgs e);
    }
}
