using System;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "consumerModeExchange"; 

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
            channel.ExchangeDeclare(exchange, ExchangeType.Direct, true, false, null);

            // Publish Msg
            for (int i = 0; i < 10000; i++)
            {
                var msg = Encoding.UTF8.GetBytes($"{i}: Hello");
                channel.BasicPublish(exchange, "", null, msg);
            }
        }
    }
}
