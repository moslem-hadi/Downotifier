using Notifier.Models.Events;
using Notifier.Services.Notify;

namespace Notifier.Services;

public class NotifierContext
{
    private INotifyService _notifyService;

    public void SetNotifyService(INotifyService notifyService)
    {
        _notifyService = notifyService;
    }

    public Task Notify(NotificationEvent notification, CancellationToken cancellationToken = default)
        => _notifyService.Notify(notification, cancellationToken);

}
