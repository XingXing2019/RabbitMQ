using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmailConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "fanoutExchange";
            var emailQueue = "emailQueue";

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
            channel.ExchangeDeclare(exchange, ExchangeType.Fanout, true, false, null);

            // Declare queue
            channel.QueueDeclare(emailQueue, true, false, false, null);

            // Bind queue
            channel.QueueBind(emailQueue, exchange, "", null);

            // Create consumer
            var emailConsumer = new EventingBasicConsumer(channel);

            emailConsumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine($"Email: {msg}");
            };

            channel.BasicConsume(emailQueue, true, emailConsumer);
            Console.Read();
        }
    }
}
