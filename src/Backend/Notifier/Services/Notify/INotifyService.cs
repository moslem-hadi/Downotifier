using Notifier.Models.Events;

namespace Notifier.Services.Notify;

public interface INotifyService
{
    Task Notify(IServiceProvider serviceProvider,NotificationEvent notification, CancellationToken cancellationToken = default);
}
