using Shared.Messaging;

namespace Scheduler.Models.Events;

public record ApiCallJobCreatedEvent : IMessage
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }

    public ApiMethod Method { get; set; }

    public string? JsonBody { get; set; }

    public Dictionary<string, string>? Headers { get; set; }

    public int MonitoringInterval { get; set; }
}

public enum ApiMethod
{
    Get,
    Post
}