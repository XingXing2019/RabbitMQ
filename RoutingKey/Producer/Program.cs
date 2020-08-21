using System;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "directExchange";
            var elseQueue = "log_else";
            var errorQueue = "log_error";

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

            // Publish msg
            for (int i = 0; i < 100; i++)
            {
                if (i % 2 == 0)
                {
                    var msg = Encoding.UTF8.GetBytes($"{i}: Debug");
                    channel.BasicPublish(exchange, "Debug", null, msg);
                }
                else if (i % 3 == 0)
                {
                    var msg = Encoding.UTF8.GetBytes($"{i}: Info");
                    channel.BasicPublish(exchange, "Info", null, msg);
                }
                else if (i % 5 == 0)
                {
                    var msg = Encoding.UTF8.GetBytes($"{i}: Warning");
                    channel.BasicPublish(exchange, "Warning", null, msg);
                }
                else
                {
                    var msg = Encoding.UTF8.GetBytes($"{i}: Error");
                    channel.BasicPublish(exchange, "Error", null, msg);
                }
            }
        }
    }
}
