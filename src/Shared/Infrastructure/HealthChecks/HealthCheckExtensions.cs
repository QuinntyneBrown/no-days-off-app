using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.HealthChecks;

/// <summary>
/// Extension methods for health check configuration
/// </summary>
public static class HealthCheckExtensions
{
    public static IHealthChecksBuilder AddRedisHealthCheck(
        this IHealthChecksBuilder builder,
        string name = "redis",
        params string[] tags)
    {
        return builder.AddCheck<RedisHealthCheck>(name, tags: tags);
    }
}
