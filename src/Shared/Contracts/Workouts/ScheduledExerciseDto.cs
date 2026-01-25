namespace Shared.Contracts.Workouts;

public sealed record ScheduledExerciseDto(
    int ScheduledExerciseId,
    string Name,
    int DayId,
    int ExerciseId,
    int Sort,
    int? Repetitions,
    int? WeightInKgs,
    int? Sets,
    int? Distance,
    int? TimeInSeconds,
    int? TenantId,
    DateTime CreatedOn);

public sealed record CreateScheduledExerciseDto(
    string Name,
    int DayId,
    int ExerciseId,
    int Sort = 0,
    int? Repetitions = null,
    int? WeightInKgs = null,
    int? Sets = null,
    int? Distance = null,
    int? TimeInSeconds = null,
    int? TenantId = null);

public sealed record UpdateScheduledExerciseDto(
    int ScheduledExerciseId,
    string Name,
    int Sort,
    int? Repetitions,
    int? WeightInKgs,
    int? Sets,
    int? Distance,
    int? TimeInSeconds);
