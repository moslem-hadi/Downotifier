using Microsoft.Extensions.DependencyInjection;

namespace Shared.Messaging.Pulsar;

public static class Extensions
{
    public static IServiceCollection AddPulsar(this IServiceCollection services)
        => services
            .AddSingleton<IMessagePublisher, PulsarMessagePublisher>()
            .AddSingleton<IMessageSubscriber, PulsarMessageSubscriber>();
}