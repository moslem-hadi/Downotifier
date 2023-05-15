namespace Domain.Events;

public record ApiCallJobCreatedEvent(ApiCallJob apiCallJob) : BaseEvent
{
}
