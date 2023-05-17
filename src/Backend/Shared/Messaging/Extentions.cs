using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging.RabbitMQ;

namespace Shared.Messaging;

public static class Extentions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
        => services
        .AddSingleton<IMessagePublisher, DefaultMessagePublisher>()
        .AddSingleton<IMessageSubscriber, DefaultMessageSubscriber>();

    public static IServiceCollection AddRabbitMQMessaging(this IServiceCollection services)
        => services
        .AddSingleton<IMessagePublisher, RabbitMqPublisher>()
        .AddSingleton<IMessageSubscriber, RabbitMqSubscriber>();
}
