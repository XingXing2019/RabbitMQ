using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var propertyQueue = "msgPropertyQueue";
            var lazyQueue = "lazyQueue";

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
            channel.QueueDeclare(propertyQueue, true, false, false, null);

            // Declare lazy queue
            channel.QueueDeclare(lazyQueue, true, false, false, new Dictionary<string, object>
            {
                {"x-queue-mode", "lazy"}
            });

            // Create basic property
            var property = channel.CreateBasicProperties();
            
            // Set msg persistent, msg will be kept if the Rabbit service is restarted
            property.Persistent = true;

            // Publish msg to msgPropertyQueue
            for (int i = 0; i < 10; i++)
            {
                var msg = $"{string.Join(",", Enumerable.Range(0, 100000))}";
                channel.BasicPublish("", propertyQueue, property, Encoding.UTF8.GetBytes("Hello"));
            }

            // Publish msg to lazyQueue
            for (int i = 0; i < 10; i++)
            {
                var msg = $"{string.Join(",", Enumerable.Range(0, 100000))}";
                channel.BasicPublish("", lazyQueue, property, Encoding.UTF8.GetBytes(msg));
                Console.WriteLine($"{i}: Finish");
            }
        }
    }
}
