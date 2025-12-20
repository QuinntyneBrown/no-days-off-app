using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoDaysOff.Core;
using NoDaysOff.Infrastructure.Data;

namespace NoDaysOff.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NoDaysOffContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(NoDaysOffContext).Assembly.FullName)));

        services.AddScoped<INoDaysOffContext>(provider => provider.GetRequiredService<NoDaysOffContext>());

        return services;
    }
}
