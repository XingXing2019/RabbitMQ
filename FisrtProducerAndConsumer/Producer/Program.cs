using System;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create factory
            var factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                UserName = "XingXing",
                Password = "XingXing"
            };

            // Create connection
            var connection = factory.CreateConnection();

            // Create Channel
            var channel = connection.CreateModel();

            // Declare exchange, use default exchange (AMQP default) exchange if no declare

            // Declare queue (durable, not exclusive, not autoDelete, no argument)
            channel.QueueDeclare("myTest", true, false, false, null);

            // Create message
            var msg = Encoding.UTF8.GetBytes("Hello RabbitMQ");

            // Publish message (use default exchange, routeKey is "myTest")
            channel.BasicPublish(string.Empty, "myTest", null, msg);

            connection.Dispose();
            channel.Dispose();
        }
    }
}
