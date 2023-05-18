namespace Domain.Events;

public record ApiCallJobUpdatedEvent(ApiCallJob apiCallJob) : BaseEvent
{
}
