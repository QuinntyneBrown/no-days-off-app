using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoDaysOff.Shared.Messaging;
using StackExchange.Redis;

namespace NoDaysOff.Shared;

/// <summary>
/// Extension methods for registering shared messaging services
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddSharedMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var messagingConfig = configuration.GetSection("Messaging");
        var provider = messagingConfig.GetValue<string>("Provider") ?? "Udp";

        if (provider.Equals("Redis", StringComparison.OrdinalIgnoreCase))
        {
            var redisConnection = messagingConfig.GetValue<string>("RedisConnection") 
                ?? throw new InvalidOperationException("RedisConnection configuration is required when using Redis provider");
            
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
            services.AddSingleton<IMessageBus, RedisMessageBus>();
        }
        else
        {
            var basePort = messagingConfig.GetValue<int>("UdpBasePort", 5000);
            services.AddSingleton<IMessageBus>(sp => 
                new UdpMessageBus(sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<UdpMessageBus>>(), basePort));
        }

        return services;
    }
}
