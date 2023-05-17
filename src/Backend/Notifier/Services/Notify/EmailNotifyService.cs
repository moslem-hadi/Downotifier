using Notifier.Models.Events;
using Shared.Services.Email;

namespace Notifier.Services.Notify
{
    public class EmailNotifyService : INotifyService
    {
        private readonly IEmailSender _emailSender;

        public EmailNotifyService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Notify(NotificationEvent notification, CancellationToken cancellationToken = default)
        {
           await  _emailSender.SendMailAsync(new Recipient(notification.Receiver), "Notifier", notification.Message, cancellationToken);
        }
    }
}
