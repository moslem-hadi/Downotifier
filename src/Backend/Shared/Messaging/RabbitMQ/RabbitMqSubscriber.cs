using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Shared.Messaging.RabbitMQ;

public sealed class RabbitMqSubscriber : IMessageSubscriber
{
    private readonly IConfiguration configuration;
    private readonly ILogger<RabbitMqPublisher> logger;

    public RabbitMqSubscriber(IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
    {
        this.configuration = configuration;
        this.logger = logger;
    }

    Task IMessageSubscriber.SubscribeAsync<T>(string queue, Action<MessageEnvelope<T>> handler)
    {
        try
        {
            IConnection _connection;
            IModel _channel;
            var factory = new ConnectionFactory
            {
                HostName = configuration["EventBusConnection"],
                UserName = configuration["RABBITMQ_DEFAULT_USER"],
                Password = configuration["RABBITMQ_DEFAULT_PASS"],
                Port = 5672
            };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare($"{queue}.exchange", ExchangeType.Topic);
            _channel.QueueDeclare(queue, false, false, false, null);
            _channel.QueueBind(queue, $"{queue}.exchange", $"{queue}.queue.*", null);
            _channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message  
                //var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                handler(new MessageEnvelope<T>(message, ""));

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue, false, consumer);
        }
        catch(Exception ex)
        {
            logger.LogCritical("errrrrrrrrrrrrrrrrrrrrrrrrrrrror");
            logger.LogCritical(ex.Message);
            logger.LogCritical(ex.InnerException?.Message ??"------");
        }
        return Task.CompletedTask;
    }
}
