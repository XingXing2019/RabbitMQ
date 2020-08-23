using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AnyConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "headerExchange";
            var anyQueue = "anyQueue";

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

            // Create header argument
            var dict = new Dictionary<string, object>();
            dict.Add("x-match", "any");
            dict.Add("username", "Jack");
            dict.Add("password", "123");

            // Declare exchange
            channel.ExchangeDeclare(exchange, ExchangeType.Headers, true, false, null);

            // Declare queue
            channel.QueueDeclare(anyQueue, true, false, false, null);

            // Bind queue
            channel.QueueBind(anyQueue, exchange, "", dict);

            // Create consumer
            var anyConsumer = new EventingBasicConsumer(channel);

            anyConsumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine($"AnyConsumer: {msg}");
            };

            channel.BasicConsume(anyQueue, true, anyConsumer);

            Console.Read();
        }
    }
}
