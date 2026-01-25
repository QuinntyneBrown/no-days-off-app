using Microsoft.EntityFrameworkCore;
using Workouts.Core.Aggregates.Day;
using Workouts.Core.Aggregates.ScheduledExercise;
using Workouts.Core.Aggregates.Workout;

namespace Workouts.Core;

public interface IWorkoutsDbContext
{
    DbSet<Workout> Workouts { get; }
    DbSet<Day> Days { get; }
    DbSet<ScheduledExercise> ScheduledExercises { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
