using Microsoft.Extensions.Logging;
using Shared.Messaging;
using static Shared.Constants;

namespace Application.ApiCallJobCommandQuery.EventHandlers;

internal class ApiCallJobUpdatedEventHandler : INotificationHandler<ApiCallJobUpdatedEvent>
{
    private readonly ILogger<ApiCallJobUpdatedEvent> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public ApiCallJobUpdatedEventHandler(ILogger<ApiCallJobUpdatedEvent> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public Task Handle(ApiCallJobUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _messagePublisher.PublishAsync(QueueConstants.JobUpdateQueue, notification.apiCallJob);

        return Task.CompletedTask;
    }

}