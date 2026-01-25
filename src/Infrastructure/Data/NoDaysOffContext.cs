using Microsoft.EntityFrameworkCore;
using Core;
using Core.Model.AthleteAggregate;
using Core.Model.BodyPartAggregate;
using Core.Model.ConversationAggregate;
using Core.Model.DashboardAggregate;
using Core.Model.DayAggregate;
using Core.Model.DigitalAssetAggregate;
using Core.Model.ExerciseAggregate;
using Core.Model.ProfileAggregate;
using Core.Model.ScheduledExerciseAggregate;
using Core.Model.TenantAggregate;
using Core.Model.TileAggregate;
using Core.Model.VideoAggregate;
using Core.Model.WorkoutAggregate;

namespace Infrastructure.Data;

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
