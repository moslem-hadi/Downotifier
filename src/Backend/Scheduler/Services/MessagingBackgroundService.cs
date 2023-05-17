using Scheduler.Models.Events;
using Shared.Messaging;

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
            stoppingToken.ThrowIfCancellationRequested();
            _messageSubscriber.SubscribeAsync<ApiCallJobCreatedEvent>("job", Fu);
            return Task.CompletedTask;

        }
        public void Fu(MessageEnvelope<ApiCallJobCreatedEvent> message)
        {
            _logger.LogInformation(message.Message.Url);
        }
    }
}
