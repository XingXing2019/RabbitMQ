using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ElseConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "directExchange";
            var elseQueue = "log_else";

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
            channel.QueueDeclare(elseQueue, true, false, false, null);

            // Bind queue
            var routingKeys = new string[] {"Debug", "Info", "Warning"};
            for (int i = 0; i < routingKeys.Length; i++)
                channel.QueueBind(elseQueue, exchange, routingKeys[i], null);

            // Create consumer
            var consumer = new EventingBasicConsumer(channel);

            // Consume msg
            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
            };

            channel.BasicConsume(elseQueue, true, consumer);
            Console.Read();
        }
    }
}
