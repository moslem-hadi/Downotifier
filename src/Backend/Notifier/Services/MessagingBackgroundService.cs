using Notifier.Models.Events;
using Notifier.Services.Notify;
using Shared.Helper;
using Shared.Messaging;
using Shared.Services.Email;
using static Shared.Constants;

namespace Notifier.Services
{
    internal sealed class MessagingBackgroundService : BackgroundService
    {
        readonly IMessageSubscriber _messageSubscriber;
        readonly ILogger<MessagingBackgroundService> _logger;
        private readonly IEmailSender _emailSender;
        public MessagingBackgroundService(IMessageSubscriber messageSubscriber, ILogger<MessagingBackgroundService> logger, IEmailSender emailSender)
        {
            _messageSubscriber = messageSubscriber;
            _logger = logger;
            _emailSender = emailSender;
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
            INotifyService strategy = default!;
            switch (notify.Type)
            {
                case NotificationType.Email:
                    strategy = new EmailNotifyService(_emailSender);
                    break;
                default:
                    break;
            }
            strategy.Notify(notify);
        }
    }
}
