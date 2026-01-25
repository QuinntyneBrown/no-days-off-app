using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Workouts.Core;
using Workouts.Infrastructure.Data;

namespace Workouts.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddWorkoutsInfrastructureWithAspire(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<WorkoutsDbContext>("workoutsdb");
        builder.Services.AddScoped<IWorkoutsDbContext>(sp => sp.GetRequiredService<WorkoutsDbContext>());
        return builder;
    }
}
