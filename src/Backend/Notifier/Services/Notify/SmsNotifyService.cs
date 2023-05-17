using Microsoft.AspNetCore.Mvc;
using Notifier.Models.Events;

namespace Notifier.Services.Notify
{
    public class SmsNotifyService : INotifyService
    {
        public Task Notify([FromServices] IServiceProvider serviceProvider, NotificationEvent notification, CancellationToken cancellationToken = default)
        {
            //Send sms-

            return Task.CompletedTask;
        }
    }
}
