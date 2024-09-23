using Locacao.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class MotorcycleEventConsumer
{
    private readonly IConnection _connection;
    private readonly IServiceProvider _serviceProvider;

    public MotorcycleEventConsumer(IConnection connection, IServiceProvider serviceProvider)
    {
        _connection = connection;
        _serviceProvider = serviceProvider;
    }

    public void StartConsuming()
    {
        using var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: "motorcycle_registration_queue",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var motorcycle = JsonSerializer.Deserialize<Motorcycle>(message);

            if (motorcycle != null && motorcycle.Year == 2024)
            {
                // Processa a moto e salva no banco de dados
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<Locacao.Infraestructure.LocacaoDbContext>();
                dbContext.Motorcycles.Add(motorcycle);
                dbContext.SaveChanges();
            }
        };

        channel.BasicConsume(queue: "motorcycle_registration_queue",
                             autoAck: true,
                             consumer: consumer);
    }
}