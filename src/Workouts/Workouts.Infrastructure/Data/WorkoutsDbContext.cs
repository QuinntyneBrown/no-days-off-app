using Microsoft.EntityFrameworkCore;
using Workouts.Core;
using Workouts.Core.Aggregates.Day;
using Workouts.Core.Aggregates.ScheduledExercise;
using Workouts.Core.Aggregates.Workout;

namespace Workouts.Infrastructure.Data;

public class WorkoutsDbContext : DbContext, IWorkoutsDbContext
{
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Day> Days => Set<Day>();
    public DbSet<ScheduledExercise> ScheduledExercises => Set<ScheduledExercise>();

    public WorkoutsDbContext(DbContextOptions<WorkoutsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkoutsDbContext).Assembly);
    }
}
