using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Authentication;
using Shared.Messaging;
using StackExchange.Redis;

namespace Shared.Infrastructure;

/// <summary>
/// Extension methods for registering shared services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSharedMessaging(configuration);
        services.AddJwtAuthentication(configuration);

        return services;
    }

    public static IServiceCollection AddSharedMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("redis");

        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(connectionString));
            services.AddSingleton<IMessageBus, RedisMessageBus>();
        }
        else
        {
            // Fallback for development without Aspire
            var messagingConfig = configuration.GetSection("Messaging");
            var provider = messagingConfig.GetValue<string>("Provider") ?? "Redis";

            if (provider.Equals("Redis", StringComparison.OrdinalIgnoreCase))
            {
                var redisConnection = messagingConfig.GetValue<string>("RedisConnection")
                    ?? "localhost:6379";

                services.AddSingleton<IConnectionMultiplexer>(
                    ConnectionMultiplexer.Connect(redisConnection));
                services.AddSingleton<IMessageBus, RedisMessageBus>();
            }
        }

        return services;
    }

    public static IServiceCollection AddSharedMessagingWithAspire(
        this IServiceCollection services)
    {
        // When using Aspire, the IConnectionMultiplexer is already registered
        services.AddSingleton<IMessageBus, RedisMessageBus>();
        return services;
    }
}
