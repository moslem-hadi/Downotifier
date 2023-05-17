using Notifier.Models.Events;
using Shared.Helper;
using Shared.Messaging;
using static Shared.Constants;

namespace Notifier.Services
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
            _messageSubscriber.SubscribeAsync<NotificationEvent>(QueueConstants.Job, HandleNotify);
            return Task.CompletedTask;

        }
        public void HandleNotify(MessageEnvelope<NotificationEvent> message)
        {
            var notify = message.Message;

        }
    }
}
