using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging;
using static Shared.Constants;

namespace Application.ApiCallJobCommandQuery.EventHandlers;

internal class ApiCallJobCreatedEventHandler : INotificationHandler<ApiCallJobCreatedEvent>
{
    private readonly ILogger<ApiCallJobCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public ApiCallJobCreatedEventHandler(ILogger<ApiCallJobCreatedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public Task Handle(ApiCallJobCreatedEvent notification, CancellationToken cancellationToken)
    {
        _messagePublisher.PublishAsync(QueueConstants.Job, notification.apiCallJob);

        return Task.CompletedTask;
    }

}