using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging;

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
        _messagePublisher.PublishAsync("job", new JobCreated
        {
            Url = notification.apiCallJob.Url,
            Id = notification.apiCallJob.Id,
            MonitoringInterval = notification.apiCallJob.MonitoringInterval,
        });

        return Task.CompletedTask;
    }

    public class JobCreated : IMessage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int MonitoringInterval { get; set; }

    }
}