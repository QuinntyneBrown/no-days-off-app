using Microsoft.EntityFrameworkCore;
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

namespace Core;

public interface INoDaysOffContext
{
    DbSet<Athlete> Athletes { get; }
    DbSet<BodyPart> BodyParts { get; }
    DbSet<Conversation> Conversations { get; }
    DbSet<Dashboard> Dashboards { get; }
    DbSet<Day> Days { get; }
    DbSet<DigitalAsset> DigitalAssets { get; }
    DbSet<Exercise> Exercises { get; }
    DbSet<Profile> Profiles { get; }
    DbSet<ScheduledExercise> ScheduledExercises { get; }
    DbSet<Tenant> Tenants { get; }
    DbSet<Tile> Tiles { get; }
    DbSet<Video> Videos { get; }
    DbSet<Workout> Workouts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
