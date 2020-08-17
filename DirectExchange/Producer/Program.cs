using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "directExchange";
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

            // Declare exchange
            channel.ExchangeDeclare(exchange, ExchangeType.Direct, true, false, null);

            // Declare queue
            channel.QueueDeclare(queue, true, false, false, null);

            // Bind queue
            channel.QueueBind(queue, exchange, "myTest", null);

            // Publish msg
            for (int i = 0; i < 100; i++)
            {
                var msg = Encoding.UTF8.GetBytes($"{i}, Hello");
                channel.BasicPublish("directExchange", "myTest", null, msg);
            }
        }
    }
}
