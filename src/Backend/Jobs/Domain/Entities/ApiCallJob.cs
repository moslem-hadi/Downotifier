using Domain.Enums;

namespace Domain.Entities;

public class ApiCallJob : BaseAuditableEntity<int>
{
   // public Guid UserId { get; set; }

    public string Title { get; set; }

    public string Url { get; set; }

    public ApiMethod Method { get; set; }

    public string? JsonBody { get; set; }

    public Dictionary<string, string>? Headers { get; set; }

    public int MonitoringInterval { get; set; }

   // public List<Notification> Notifications{ get; set; }

}
public class Notification
{
    //save the notifications.
}