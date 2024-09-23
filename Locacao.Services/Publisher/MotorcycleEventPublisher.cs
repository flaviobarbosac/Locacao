using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Locacao.Domain.Model;

namespace Locacao.Services.Publisher
{
    public class MotorcycleEventPublisher
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public MotorcycleEventPublisher(IConfiguration configuration)
        {
            _hostname = configuration["RabbitMQ:HostName"];
            _queueName = configuration["RabbitMQ:QueueName"];
            _username = configuration["RabbitMQ:UserName"];
            _password = configuration["RabbitMQ:Password"];
        }

        public void PublishMotorcycleRegisteredEvent(Motorcycle motorcycle)
        {
            var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var message = JsonConvert.SerializeObject(motorcycle);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
            }
        }
    }
}