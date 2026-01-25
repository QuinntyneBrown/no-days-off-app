using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Communication.Core;
using Communication.Core.Services;
using Communication.Infrastructure.Data;
using Communication.Infrastructure.Services;

namespace Communication.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddCommunicationInfrastructureWithAspire(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<CommunicationDbContext>("communicationdb");
        builder.Services.AddScoped<ICommunicationDbContext>(sp => sp.GetRequiredService<CommunicationDbContext>());
        builder.Services.AddScoped<INotificationService, SignalRNotificationService>();
        return builder;
    }
}
