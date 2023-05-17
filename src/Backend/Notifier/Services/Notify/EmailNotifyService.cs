using Microsoft.AspNetCore.Mvc;
using Notifier.Models.Events;
using ProtoBuf.Meta;
using Shared.Services.Email;

namespace Notifier.Services.Notify
{
    public class EmailNotifyService : INotifyService
    {
        public EmailNotifyService()
        {
        }

        public async Task Notify([FromServices]IServiceProvider serviceProvider,NotificationEvent notification, CancellationToken cancellationToken = default)
        {
            var _emailSender = serviceProvider.GetRequiredService<IEmailSender>();

            await  _emailSender.SendMailAsync(new Recipient(notification.Receiver), "Notifier", notification.Message, cancellationToken);
        }
    }
}
