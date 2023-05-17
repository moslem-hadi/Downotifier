namespace Shared.Services.Email;

public class MailerSendOptions
{
    public string? ApiToken { get; set; }
    public string? SenderEmail { get; set; }
    public string? SenderName { get; set; }
}
