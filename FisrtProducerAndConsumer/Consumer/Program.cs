using System;
using System.Text;
using RabbitMQ.Client;

namespace Consumer
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

            // Create channel
            var channel = connection.CreateModel();

            // Get message (get message from "myTest" queue with autoAck to delete msg after consuming)
            var result = channel.BasicGet("myTest", true);

            // Encoding result
            var msg = Encoding.UTF8.GetString(result.Body);

            // Display msg
            Console.WriteLine(msg);
        }
    }
}
