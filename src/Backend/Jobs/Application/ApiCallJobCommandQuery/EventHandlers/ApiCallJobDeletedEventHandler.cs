using Microsoft.Extensions.Logging;
using Shared.Messaging;
using static Shared.Constants;

namespace Application.ApiCallJobCommandQuery.EventHandlers;

internal class ApiCallJobDeletedEventHandler : INotificationHandler<ApiCallJobDeletedEvent>
{
    private readonly ILogger<ApiCallJobDeletedEvent> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public ApiCallJobDeletedEventHandler(ILogger<ApiCallJobDeletedEvent> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public Task Handle(ApiCallJobDeletedEvent notification, CancellationToken cancellationToken)
    {
        _messagePublisher.PublishAsync(QueueConstants.JobDeleteQueue, notification.apiCallJob);

        return Task.CompletedTask;
    }

}