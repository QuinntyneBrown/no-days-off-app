using Athletes.Core;
using Athletes.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Athletes.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddAthletesInfrastructureWithAspire(
        this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<AthletesDbContext>("athletesdb");

        builder.Services.AddScoped<IAthletesDbContext>(provider =>
            provider.GetRequiredService<AthletesDbContext>());

        return builder;
    }
}
