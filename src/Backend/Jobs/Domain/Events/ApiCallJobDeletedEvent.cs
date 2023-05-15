namespace Domain.Events;

public record ApiCallJobDeletedEvent(ApiCallJob apiCallJob) : BaseEvent
{
}
