using Microsoft.EntityFrameworkCore;
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

namespace NoDaysOff.Core;

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
