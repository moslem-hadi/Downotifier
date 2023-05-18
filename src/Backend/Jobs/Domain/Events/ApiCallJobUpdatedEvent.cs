namespace Domain.Events;

public class ApiCallJobUpdatedEvent : BaseEvent
{
    public ApiCallJob apiCallJob{ get; }
    public ApiCallJobUpdatedEvent(ApiCallJob apiCallJob)
    {
        this.apiCallJob = apiCallJob;
    }
}
