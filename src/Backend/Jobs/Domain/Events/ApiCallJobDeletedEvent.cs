namespace Domain.Events;

public class ApiCallJobDeletedEvent : BaseEvent
{
    public ApiCallJob apiCallJob { get; }
    public ApiCallJobDeletedEvent(ApiCallJob apiCallJob)
    {
        this.apiCallJob = apiCallJob;
    }
}
