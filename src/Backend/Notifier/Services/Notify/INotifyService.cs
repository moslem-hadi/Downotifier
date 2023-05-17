using Notifier.Models.Events;

namespace Notifier.Services.Notify;

public interface INotifyService
{
    Task Notify(NotificationEvent notification, CancellationToken cancellationToken = default);
}
