using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var queue = "ackQueue";

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

            // Confirm publish
            channel.ConfirmSelect();

            // Publish msg
            for (int i = 0; i < 15; i++)
            {
                channel.BasicPublish("", queue, null, Encoding.UTF8.GetBytes($"{i}: Hello"));
            }

            // Wait for confirmation 
            var isAllPublished = channel.WaitForConfirms(new TimeSpan(0, 0, 0, 1));
        }
    }
}
