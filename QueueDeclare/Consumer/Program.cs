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
            var exchange = "queueDeclareExchange";
            var durableQueue = "durableQueue";
            var exclusiveQueue = "exclusiveQueue";
            var autoDeleteQueue = "autoDeleteQueue";
            var passiveQueue = "passiveQueue";

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


            // Declare durable queue
            channel.QueueDeclare(durableQueue, true, false, false, null);


            // Declare exclusive queue, the queue will be deleted after connection is closed
            channel.QueueDeclare(exclusiveQueue, true, true, false, null);


            // Declare auto delete queue, the queue will be deleted when the last consumer unsubscribes
            channel.QueueDeclare(autoDeleteQueue, true, false, true, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
            };
            channel.BasicConsume(autoDeleteQueue, true, consumer);
            

            // Declare passive queue
            channel.QueueDeclare(passiveQueue, true, false, false, null);
            // QueueDeclarePassive only check if queue exists
            channel.QueueDeclarePassive(passiveQueue);

            
            Console.Read();
        }
    }
}
