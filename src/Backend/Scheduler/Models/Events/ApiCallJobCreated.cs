using Shared.Enums;
using Shared.Messaging;

namespace Scheduler.Models.Events;

public record ApiCallJob
{
    public int Id { get; set; }

    public string Url { get; set; }

    public ApiMethod Method { get; set; }

    public string? JsonBody { get; set; }

    public Dictionary<string, string>? Headers { get; set; }

    public int MonitoringInterval { get; set; }

    public List<Notification> Notifications { get; set; }
}

public record ApiCallJobCreated: ApiCallJob{ }
public record ApiCallJobUpdated : ApiCallJob { }
public record ApiCallJobDeleted : ApiCallJob { }
public class Notification// : IMessage
{
    public NotificationType Type { get; set; }
    public string Receiver { get; set; }
    public string Message { get; set; }
}

