using Shared.Messaging;

namespace Scheduler.Models.Events;

public record ApiCallJobCreatedEvent: IMessage
{
        public int Id { get; set; }
    public string Url { get; set; }
}
