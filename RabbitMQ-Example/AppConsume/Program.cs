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
            IConnection connection = Factory.ICreateConnection(Settings.GetFactorySettings());
            Factory.CreateQueue(queuneName);

            using (IModel channel = connection.CreateModel())
            {
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
