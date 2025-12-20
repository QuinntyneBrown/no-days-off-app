using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.AthleteAggregate;
using NoDaysOff.Core.Model.BodyPartAggregate;
using NoDaysOff.Core.Model.ConversationAggregate;
using NoDaysOff.Core.Model.DashboardAggregate;
using NoDaysOff.Core.Model.DayAggregate;
using NoDaysOff.Core.Model.DigitalAssetAggregate;
using NoDaysOff.Core.Model.ExerciseAggregate;
using NoDaysOff.Core.Model.ProfileAggregate;
using NoDaysOff.Core.Model.ScheduledExerciseAggregate;
using NoDaysOff.Core.Model.TenantAggregate;
using NoDaysOff.Core.Model.TileAggregate;
using NoDaysOff.Core.Model.VideoAggregate;
using NoDaysOff.Core.Model.WorkoutAggregate;

namespace NoDaysOff.Infrastructure.Data;

public class NoDaysOffContext : DbContext, INoDaysOffContext
{
    public NoDaysOffContext(DbContextOptions<NoDaysOffContext> options)
        : base(options)
    {
    }

    public DbSet<Athlete> Athletes => Set<Athlete>();
    public DbSet<BodyPart> BodyParts => Set<BodyPart>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Dashboard> Dashboards => Set<Dashboard>();
    public DbSet<Day> Days => Set<Day>();
    public DbSet<DigitalAsset> DigitalAssets => Set<DigitalAsset>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<ScheduledExercise> ScheduledExercises => Set<ScheduledExercise>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Tile> Tiles => Set<Tile>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Workout> Workouts => Set<Workout>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoDaysOffContext).Assembly);
    }
}
