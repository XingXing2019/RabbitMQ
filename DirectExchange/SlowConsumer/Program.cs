using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SlowConsumer
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

            // Only one msg each time before msg is ack
            channel.BasicQos(0, 1, false);

            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);

                // Ack msg
                channel.BasicAck(e.DeliveryTag, true);
            };

            // Turn off autoAck
            channel.BasicConsume(queue, false, consumer);

            Console.Read();
        }
    }
}
