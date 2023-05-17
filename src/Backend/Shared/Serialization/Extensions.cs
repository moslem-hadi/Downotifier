using Microsoft.Extensions.DependencyInjection;

namespace Shared.Serialization;

public static class Extensions
{
    public static IServiceCollection AddSerialization(this IServiceCollection services)
        => services.AddSingleton<ISerializer, SystemTextJsonSerializer>();
}