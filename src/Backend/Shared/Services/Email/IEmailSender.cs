namespace Shared.Services.Email;

public interface IEmailSender
{
    Task SendMailAsync(Recipient to, string? subject = null, string? text = null, CancellationToken cancellationToken = default);
}
