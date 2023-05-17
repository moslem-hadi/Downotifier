using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Shared.Messaging.RabbitMQ;

public sealed class RabbitMqSubscriber : IMessageSubscriber
{
    Task IMessageSubscriber.SubscribeAsync<T>(string queue, Action<MessageEnvelope<T>> handler)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) => {
            var body = eventArgs.Body.ToArray();
            var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

            handler(new MessageEnvelope<T>(message, ""));
        };
        //read the message
        channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }
}
