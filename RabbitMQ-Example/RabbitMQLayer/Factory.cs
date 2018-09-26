﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Global;

namespace RabbitMQLayer
{
    public static class Factory
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;

        public static IConnection CreateConnection(FactorySettings settings)
        {
            _factory = new ConnectionFactory();
            _factory.HostName = settings.HostName;
            _factory.Port = settings.Port;
            _factory.UserName = settings.UserName;
            _factory.Password = settings.Password;
            _factory.VirtualHost = settings.VirtualHost;
            return _factory.CreateConnection();
        }

        public static void Publish(FactorySettings settings, string queueName, object obj)
        {
            _connection = CreateConnection(settings);
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
