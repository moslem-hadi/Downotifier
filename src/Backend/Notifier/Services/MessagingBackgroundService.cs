using Notifier.Models.Events;
using Notifier.Services.Notify;
using Shared.Enums;
using Shared.Messaging;
using static Shared.Constants;

namespace Notifier.Services
{
    internal sealed class MessagingBackgroundService : BackgroundService
    {
        readonly IMessageSubscriber _messageSubscriber;
        private readonly INotifierContext _notifierContext;
        public MessagingBackgroundService(IMessageSubscriber messageSubscriber, INotifierContext notifierContext)
        {
            _messageSubscriber = messageSubscriber;
            _notifierContext = notifierContext;
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
            //TODO: make it better
            switch (notify.Type)
            {
                case NotificationType.Email:
                    strategy = new EmailNotifyService();
                    break;
                default:
                    break;
            }

            _notifierContext.SetNotifyService(strategy);
            _notifierContext.Notify(notify);
        }
    }
}
