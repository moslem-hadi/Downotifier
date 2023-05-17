using Notifier.Models.Events;
using Notifier.Services.Notify;
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
            _messageSubscriber.SubscribeAsync<NotificationEvent>(QueueConstants.Notify, HandleNotify);
            return Task.CompletedTask;

        }
        public void HandleNotify(MessageEnvelope<NotificationEvent> message)
        {
            var notify = message.Message;
            var notifier = new NotifierContext(new SmsNotifyService());

            notifier.Notify(notify);
        }
    }
}
