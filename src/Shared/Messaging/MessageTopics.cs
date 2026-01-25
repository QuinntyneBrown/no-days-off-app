namespace Shared.Messaging;

/// <summary>
/// Centralized message topic definitions
/// </summary>
public static class MessageTopics
{
    public static class Identity
    {
        public const string UserCreated = "identity.user.created";
        public const string UserUpdated = "identity.user.updated";
        public const string UserAuthenticated = "identity.user.authenticated";
        public const string TenantCreated = "identity.tenant.created";
    }

    public static class Athletes
    {
        public const string AthleteCreated = "athletes.athlete.created";
        public const string AthleteUpdated = "athletes.athlete.updated";
        public const string AthleteDeleted = "athletes.athlete.deleted";
    }

    public static class Workouts
    {
        public const string WorkoutCreated = "workouts.workout.created";
        public const string WorkoutCompleted = "workouts.workout.completed";
    }

    public static class Exercises
    {
        public const string ExerciseCreated = "exercises.exercise.created";
        public const string ExerciseUpdated = "exercises.exercise.updated";
        public const string ExerciseDeleted = "exercises.exercise.deleted";
        public const string BodyPartCreated = "exercises.bodypart.created";
    }

    public static class Dashboards
    {
        public const string DashboardCreated = "dashboards.dashboard.created";
    }

    public static class Media
    {
        public const string AssetUploaded = "media.asset.uploaded";
        public const string VideoUploaded = "media.video.uploaded";
        public const string MediaUploaded = "media.file.uploaded";
        public const string MediaDeleted = "media.file.deleted";
    }

    public static class Communication
    {
        public const string MessageSent = "communication.message.sent";
    }
}
