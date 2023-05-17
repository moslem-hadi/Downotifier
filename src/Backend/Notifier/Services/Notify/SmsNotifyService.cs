using Notifier.Models.Events;

namespace Notifier.Services.Notify
{
    public class SmsNotifyService : INotifyService, ISmsNotifyService
    {
        public Task Notify(NotificationEvent notification, CancellationToken cancellationToken = default)
        {
            //Send sms-

            return Task.CompletedTask;
        }
    }

    public interface ISmsNotifyService
    {
    }
}
