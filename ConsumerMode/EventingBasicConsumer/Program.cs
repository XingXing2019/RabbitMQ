using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace EventingBasicConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "consumerModeExchange";
            var eventConsumerQueue = "eventConsumerQueue";

            // Create factory
            var factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "XingXing",
                Password = "XingXing"
            };

            // Create connection
            var connection = factory.CreateConnection();

            // Create channel
            var channel = connection.CreateModel();

            // Declare exchange
            channel.ExchangeDeclare(exchange, ExchangeType.Direct, true, false, null);

            // Declare queue
            channel.QueueDeclare(eventConsumerQueue, true, false, false, null);

            // Bind queue
            channel.QueueBind(eventConsumerQueue, exchange, "", null);

            // Set Qos, narrowing the channel allow the consumer to consume the msg, the msg will not lose
            channel.BasicQos(0, 1, false);

            // Create consumer
            var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
                Thread.Sleep(1000 * 1);

                channel.BasicAck(e.DeliveryTag, false);
            };

            channel.BasicConsume(eventConsumerQueue, false, consumer);
            Console.Read();
        }
    }
}
