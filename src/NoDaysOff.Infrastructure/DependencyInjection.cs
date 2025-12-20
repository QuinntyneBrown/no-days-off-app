using Microsoft.Extensions.DependencyInjection;

namespace NoDaysOff.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register infrastructure services here
        return services;
    }
}
