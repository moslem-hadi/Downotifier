using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging.RabbitMQ;

namespace Shared.Messaging.RabbitMQ;

public static class Extentions
{
    public static IServiceCollection AddRabbitMQMessaging(this IServiceCollection services)
        => services
        .AddSingleton<IMessagePublisher, RabbitMqPublisher>()
        .AddSingleton<IMessageSubscriber, RabbitMqSubscriber>();
}
