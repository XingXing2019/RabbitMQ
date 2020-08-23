using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "topicExchange";

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

            // Publish msg
            for (int i = 0; i < 100; i++)
            {
                string routingKey;
                byte[] msg;
                if (i % 2 == 0)
                {
                    routingKey = "www.ctrip.com";
                    msg = Encoding.UTF8.GetBytes($"{i}: {routingKey} msg");
                }
                else
                {
                    routingKey = "www.immi.china.gov";
                    msg = Encoding.UTF8.GetBytes($"{i}: {routingKey} msg");
                }
                channel.BasicPublish(exchange, routingKey, null, msg);
            }
        }
    }
}
