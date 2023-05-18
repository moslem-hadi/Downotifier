using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Shared.Messaging.RabbitMQ;

public sealed class RabbitMqPublisher : IMessagePublisher
{
    private readonly IConfiguration configuration;
    private readonly ILogger<RabbitMqPublisher> logger;

    public RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
    {
        this.configuration = configuration;
        this.logger = logger;
    }
    public async Task PublishAsync<T>(string queue, T message) where T : class//, IMessage
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["EventBusConnection"],
            UserName = configuration["RABBITMQ_DEFAULT_USER"],
            Password = configuration["RABBITMQ_DEFAULT_PASS"],
            Port = 5672
        };
        var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: queue, body: body);
    }
}
