using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System;

namespace Shared.Messaging.RabbitMQ;

public sealed class RabbitMqSubscriber : IMessageSubscriber
{
    Task IMessageSubscriber.SubscribeAsync<T>(string queue, Action<MessageEnvelope<T>> handler)
    {
        IConnection _connection;
        IModel _channel;
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };

        // create connection  
        _connection = factory.CreateConnection();

        // create channel  
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare("job.exchange", ExchangeType.Topic);
        _channel.QueueDeclare("job", false, false, false, null);
        _channel.QueueBind("job", "job.exchange", "job.queue.*", null);
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

        _channel.BasicConsume("job", false, consumer);
        return Task.CompletedTask;
    }
}
