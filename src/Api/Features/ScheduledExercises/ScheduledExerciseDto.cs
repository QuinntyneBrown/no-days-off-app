namespace Api;

public sealed record ScheduledExerciseDto(
    int ScheduledExerciseId,
    string Name,
    int? DayId,
    int? ExerciseId,
    int Sort,
    int Repetitions,
    int WeightInKgs,
    int Sets,
    int Distance,
    int TimeInSeconds,
    DateTime CreatedOn,
    string CreatedBy);
