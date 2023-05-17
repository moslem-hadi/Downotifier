using Notifier.Models.Events;

namespace Notifier.Services.Notify
{
    public class SmsNotifyService : INotifyService
    {
        public Task Notify(NotificationEvent notification, CancellationToken cancellationToken = default)
        {
            //Send sms-

            return Task.CompletedTask;
        }
    }
}
