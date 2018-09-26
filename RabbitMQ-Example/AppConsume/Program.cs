using Global;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQLayer;
using System;
using System.Text;

namespace AppConsume
{
    class Program
    {
        static void Main()
        {
            Subscrible(Settings.QueueName);
        }
        public static void Subscrible(string queuneName)
        {
            IConnection connection = Factory.CreateConnection(Settings.GetFactorySettings());
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queuneName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Message obj = JsonConvert.DeserializeObject<Message>(message);
                    Console.WriteLine("{0}\t| {1}\t| {2}", obj.Receiver, obj.Header, obj.Body);
                };
                channel.BasicConsume(queuneName, true, consumer);
                Console.ReadLine();
            }
        }
    }
}
