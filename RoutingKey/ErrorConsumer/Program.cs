using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ErrorConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "directExchange";
            var errorQueue = "log_error";

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
            channel.QueueDeclare(errorQueue, true, false, false, null);

            // Bind queue
            channel.QueueBind(errorQueue, exchange, "Error", null);
            
            // Create consumer
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
            };

            channel.BasicConsume(errorQueue, true, consumer);
            Console.Read();
        }
    }
}
