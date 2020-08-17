using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var queue = "myTest";

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

            // Declare queue
            channel.QueueDeclare(queue, true, false, false, null);

            // EventingBasicConsumer to subscribe msg event
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
            };
            
            channel.BasicConsume(queue, true, consumer);

            Console.Read();
        }
    }
}
