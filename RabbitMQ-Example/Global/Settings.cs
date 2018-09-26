using Models;

namespace Global
{
    public static class Settings
    {
        public static string QueueName = "Examp";

        public static FactorySettings GetFactorySettings()
        {
            return new FactorySettings()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "okan",
                Password = "123456",
                VirtualHost = "/"
            };
        }
    }
}
