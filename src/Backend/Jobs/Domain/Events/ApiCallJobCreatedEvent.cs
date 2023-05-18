namespace Domain.Events;

public class ApiCallJobCreatedEvent : BaseEvent
{
    public ApiCallJob apiCallJob { get; }
    public ApiCallJobCreatedEvent(ApiCallJob apiCallJob)
    {
        this.apiCallJob = apiCallJob;
    }
}
