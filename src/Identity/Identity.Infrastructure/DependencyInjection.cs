using Identity.Core;
using Identity.Core.Services;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("identitydb"),
                b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));

        services.AddScoped<IIdentityDbContext>(provider =>
            provider.GetRequiredService<IdentityDbContext>());

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    public static IHostApplicationBuilder AddIdentityInfrastructureWithAspire(
        this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<IdentityDbContext>("identitydb");

        builder.Services.AddScoped<IIdentityDbContext>(provider =>
            provider.GetRequiredService<IdentityDbContext>());

        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<ITokenService, TokenService>();

        return builder;
    }
}
