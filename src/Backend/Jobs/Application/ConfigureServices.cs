using System.Reflection;
using Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using MediatR.NotificationPublishers;
using Shared.Messaging;
using Shared.Messaging.Pulsar;
using Shared.Serialization;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            cfg.NotificationPublisher = new TaskWhenAllPublisher();//Publish Notifications In Parallel

        });

        services.AddSerialization()
            .AddMessaging()
            .AddRabbitMQMessaging()
            ;//.AddPulsar();

        return services;
    }
}
