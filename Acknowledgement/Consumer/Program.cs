using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
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

            // Create consumer
            var consumer = new EventingBasicConsumer(channel);

            // Basic get result
            var result = channel.BasicGet(queue, false);

            // Manual ack
            //channel.BasicAck(result.DeliveryTag, false);
            Console.WriteLine(Encoding.UTF8.GetString(result.Body));

            // Reject msg
            channel.BasicReject(result.DeliveryTag, true);

            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
            };

            // Set auto ack, true to delete msg after consuming, false to keep msg after consuming
            channel.BasicConsume(queue, true, consumer);

            Console.Read();
        }
    }
}
