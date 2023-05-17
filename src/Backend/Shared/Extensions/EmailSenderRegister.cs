using Microsoft.Extensions.DependencyInjection;
using Shared.Services.Email;

namespace Shared.Extensions;

public static class EmailSenderRegister
{
    private const string mailerSendApiUrl = "https://api.mailersend.com/v1/";

    /// <summary>
    /// Call AddMailerSend and set the options:<para/>
    /// services.AddMailerSend(options => { options.ApiToken = "API-TOKEN"; options.SenderEmail = "mail@domain.com"; options.SenderName = "MailerSend"; });
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddMailerSend(
        this IServiceCollection services, Action<MailerSendOptions> configure)
    {
        services.AddMailerSend();
        services.Configure(configure);
        return services;
    }

    /// <summary>
    /// 1. Have the configs in appSettings:<para/>
    /// "MailerSend": { "ApiToken": "API-TOKEN", "SenderEmail": "mail@domain.com", "SenderName": "MailerSend"}<para/>
    /// 2. Configure the options:<para />
    /// services.Configure<MailerSendOptions>(Configuration.GetSection("MailerSend")); <para/>
    /// 3. Call AddMailerSend <para/>
    /// services.AddMailerSend();
    /// </summary>
    public static IServiceCollection AddMailerSend(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddHttpClient<IEmailSender, MailerSendEmailSender>(options =>
        {
            options.BaseAddress = new Uri(mailerSendApiUrl);
        });

        return services;
    }
}