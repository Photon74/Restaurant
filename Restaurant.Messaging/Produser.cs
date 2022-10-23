using RabbitMQ.Client;
using System.Text;

namespace Restaurant.Messaging
{
    public class Produser
    {
        private readonly string _queueName;
        private readonly string _hostName;

        public Produser(string queueName, string hostName)
        {
            _queueName = queueName;
            _hostName = hostName;
        }

        public void Send(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                "direct_exchange",
                "direct",
                false,
                false,
                null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "direct_exchange",
                routingKey: _queueName,
                basicProperties: null,
                body: body);
        }
    }
}
