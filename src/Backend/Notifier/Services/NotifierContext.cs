using Notifier.Models.Events;
using Notifier.Services.Notify;

namespace Notifier.Services;

public class NotifierContext: INotifierContext
{
    private INotifyService _notifyService;
    private readonly IServiceProvider _serviceProvider;

    public NotifierContext(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void SetNotifyService(INotifyService notifyService)
    {
        _notifyService = notifyService;
    }

    public Task Notify(NotificationEvent notification, CancellationToken cancellationToken = default)
        => _notifyService.Notify(_serviceProvider, notification, cancellationToken);

}

public interface INotifierContext
{
    void SetNotifyService(INotifyService notifyService);
    Task Notify(NotificationEvent notification, CancellationToken cancellationToken = default);
}