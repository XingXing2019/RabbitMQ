using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ComConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "topicExchange";
            var queue = "comQueue";

            // * can substitute exactly one word, # can substitute zero or more words
            var routingKey = "#.com";

            // Create factory
            var factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "XingXing",
                Password = "XingXing",
            };

            // Create connection
            var connection = factory.CreateConnection();

            // Create chaneal
            var channel = connection.CreateModel();

            // Declare exchange
            channel.ExchangeDeclare(exchange, ExchangeType.Topic, true, false, null);

            // Declare queue
            channel.QueueDeclare(queue, true, false, false, null);

            // Bind queue
            channel.QueueBind(queue, exchange, routingKey, null);

            // Create consumer 
            var comConsumer = new EventingBasicConsumer(channel);

            comConsumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine($"ComConsumer: {msg}");
            };

            channel.BasicConsume(queue, true, comConsumer);

            Console.Read();
        }
    }
}
