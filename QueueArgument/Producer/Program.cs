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
            var ttlQueue = "ttlQueue";
            var autoExpireQueue = "autoExpireQueue";
            var maxLengthQueue = "maxLengthQueue";
            var deadLetterTestQueue = "deadLetterTestQueue";
            var maxPriorityQueue = "maxPriorityQueue";

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


            // Declare ttl queue, msh will be deleted after 5000 ms
            channel.QueueDeclare(ttlQueue, false, false, false, new Dictionary<string, object>
            {
                {"x-message-ttl", 1000 * 5}
            });

            // Declare msg ttl as 3000 ms
            var expireProperty = channel.CreateBasicProperties();
            expireProperty.Expiration = "3000";
            channel.BasicPublish("", ttlQueue, expireProperty, Encoding.UTF8.GetBytes("Hello"));


            // Declare autoExpire queue, queue will be deleted after 5000 ms if queue is unused 
            channel.QueueDeclare(autoExpireQueue, false, false, false, new Dictionary<string, object>
            {
                {"x-expires", 1000 * 5}
            });


            // Declare maxLength queue, msg will be dropped from head to provide space for new msg
            channel.QueueDeclare(maxLengthQueue, false, false, false, new Dictionary<string, object>
            {
                {"x-max-length", 5}
            });
            for (int i = 0; i < 10; i++)
                channel.BasicPublish("", maxLengthQueue, null, Encoding.UTF8.GetBytes($"{i}: Hello"));


            // Declare deadLetter queue, msg will not be dropped by be sent to deadLetter queue
            var deadLetterExchange = "deadLetterExchange";
            var deadLetterQueue = "deadLetterQueue";
            
            channel.ExchangeDeclare(deadLetterExchange, ExchangeType.Direct, false, false, null);
            channel.QueueDeclare(deadLetterQueue, false, false, false, null);
            channel.QueueBind(deadLetterQueue, deadLetterExchange, deadLetterQueue, null);

            channel.QueueDeclare(deadLetterTestQueue, false, false, false, new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", deadLetterExchange},
                {"x-dead-letter-routing-key", deadLetterQueue},
                {"x-max-length", 5}
            });

            for (int i = 0; i < 10; i++)
                channel.BasicPublish("", deadLetterTestQueue, null, Encoding.UTF8.GetBytes($"{i}: Hello"));


            // Declare maxPriority queue, larger priority msg will be consumed firstly
            channel.QueueDeclare(maxPriorityQueue, false, false, false, new Dictionary<string, object>
            {
                {"x-max-priority", 10}
            });

            var priorityProperty = channel.CreateBasicProperties();

            for (int i = 0; i < 10; i++)
            {
                priorityProperty.Priority = (byte) i;
                channel.BasicPublish("", maxPriorityQueue, priorityProperty, Encoding.UTF8.GetBytes($"{i}: Hello"));
            }
        }
    }
}
