using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "headerExchange";

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

            // Create basic property
            var property = channel.CreateBasicProperties();

            for (int i = 0; i < 100; i++)
            {
                if (i % 2 == 0)
                {
                    property.Headers = new Dictionary<string, object>
                    {
                        { "username", "Jack"}
                    };
                    var msg = Encoding.UTF8.GetBytes($"{i}: Any Message");
                    channel.BasicPublish(exchange, "", property, msg);
                }
                else
                {
                    property.Headers = new Dictionary<string, object>
                    {
                        {"username", "Jack" },
                        {"password", "123" }
                    };
                    var msg = Encoding.UTF8.GetBytes($"{i}: All Message");
                    channel.BasicPublish(exchange, "", property, msg);
                }
            }
        }
    }
}
