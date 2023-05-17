using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Shared.Messaging.RabbitMQ;

public sealed class RabbitMqSubscriber : IMessageSubscriber
{
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqSubscriber()
    {
        InitRabbitMQ();
    }

    Task IMessageSubscriber.SubscribeAsync<T>(string queue, Action<MessageEnvelope<T>> handler)
    {
        //stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            // received message  
            var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

            // handle the received message  
            HandleMessage(content);
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume("job", false, consumer);
        return Task.CompletedTask;
    }



    private void InitRabbitMQ()
    {
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

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }

    

    private void HandleMessage(string content)
    {
        // we just print this message   
       // _logger.LogInformation($"consumer received {content}");
       Console.WriteLine(content);
    }

    private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
    private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
    private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
    private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }
     

}
