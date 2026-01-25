using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Exercises.Core;
using Exercises.Infrastructure.Data;

namespace Exercises.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddExercisesInfrastructureWithAspire(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<ExercisesDbContext>("exercisesdb");
        builder.Services.AddScoped<IExercisesDbContext>(sp => sp.GetRequiredService<ExercisesDbContext>());
        return builder;
    }
}
