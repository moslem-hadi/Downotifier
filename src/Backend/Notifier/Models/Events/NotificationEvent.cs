namespace Notifier.Models.Events
{
    public class NotificationEvent
    {
        public NotificationType Type { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }
    }
    //TODO: duplication
    public enum NotificationType
    {
        Email
    }
}
