using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AllConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "headerExchange";
            var allQueue = "allQueue";

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
            channel.ExchangeDeclare(exchange, ExchangeType.Headers, true, false, null);

            // Declare queue
            channel.QueueDeclare(allQueue, true, false, false, null);

            // Create header argument
            var dict = new Dictionary<string, object>();
            dict.Add("x-match", "all");
            dict.Add("username", "Jack");
            dict.Add("password", "123");

            // Declare exchange
            channel.ExchangeDeclare(exchange, ExchangeType.Headers, true, false, null);

            // Bind queue
            channel.QueueBind(allQueue, exchange, "", dict);

            // Create consumer
            var allConsumer = new EventingBasicConsumer(channel);

            allConsumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine($"AllConsumer: {msg}");
            };

            channel.BasicConsume(allQueue, true, allConsumer);

            Console.Read();
        }
    }
}
