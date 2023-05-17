using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Scheduler.Models.Events;
using Shared.Messaging;
using System.Text;
using System.Text.Json;

namespace Scheduler.Services
{
    internal sealed class MessagingBackgroundService : BackgroundService
    {
        readonly IMessageSubscriber _messageSubscriber;
        readonly ILogger<MessagingBackgroundService> _logger;
        public MessagingBackgroundService(IMessageSubscriber messageSubscriber, ILogger<MessagingBackgroundService> logger)
        {
            _messageSubscriber = messageSubscriber;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //            _messageSubscriber.SubscribeAsync<ApiCallJobCreatedEvent>("job", Fu);


            var queue = "job";

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
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = JsonSerializer.Deserialize<ApiCallJobCreatedEvent>(Encoding.UTF8.GetString(body));
                //handler.Invoke(message);
            };
            //read the message
            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);


            return Task.CompletedTask;
        }
        public void Fu(ApiCallJobCreatedEvent message)
        {
            _logger.LogInformation(message.Url);
        }
    }
}
