using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dashboard.Core;
using Dashboard.Infrastructure.Data;

namespace Dashboard.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddDashboardInfrastructureWithAspire(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<DashboardDbContext>("dashboarddb");
        builder.Services.AddScoped<IDashboardDbContext>(sp => sp.GetRequiredService<DashboardDbContext>());
        return builder;
    }
}
