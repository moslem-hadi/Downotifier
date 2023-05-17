using Microsoft.Extensions.DependencyInjection;
using Notifier.Models.Events;
using Notifier.Services.Notify;
using Shared.Helper;
using Shared.Messaging;
using Shared.Services.Email;
using System;
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
