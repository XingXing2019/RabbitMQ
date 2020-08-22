using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SmsConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "fanourExchange";
            var smsQueue = "smsQueue";

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
            channel.QueueDeclare(smsQueue, true, false, false, null);

            // Bind queue
            channel.QueueBind(smsQueue, exchange, "", null);

            // Create consumer
            var smsConsumer = new EventingBasicConsumer(channel);

            smsConsumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine($"Sms: {msg}");
            };

            channel.BasicConsume(smsQueue, true, smsConsumer);
            Console.Read();
        }
    }
}
