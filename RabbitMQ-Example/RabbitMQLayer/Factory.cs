using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQLayer
{
    public static class Factory
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        public static IConnection ICreateConnection(FactorySettings settings)
        {
            _factory = new ConnectionFactory();
            _factory.HostName = settings.HostName;
            _factory.Port = settings.Port;
            _factory.UserName = settings.UserName;
            _factory.Password = settings.Password;
            _factory.VirtualHost = settings.VirtualHost;
            _connection = _factory.CreateConnection();
            return _connection;
        }

        public static void CreateConnection(FactorySettings settings)
        {
            _factory = new ConnectionFactory();
            _factory.HostName = settings.HostName;
            _factory.Port = settings.Port;
            _factory.UserName = settings.UserName;
            _factory.Password = settings.Password;
            _factory.VirtualHost = settings.VirtualHost;
            _connection = _factory.CreateConnection();
        }

        private static void CreateQueue(string queueName)
        {
            using (IModel channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
        }

        public static void Publish(string queueName, object obj)
        {
            using (IModel channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(obj);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body);
            }
        }
    }
}
