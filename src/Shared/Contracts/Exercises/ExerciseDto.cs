namespace Shared.Contracts.Exercises;

public sealed record ExerciseDto(
    int ExerciseId,
    string Name,
    int? BodyPartId,
    int? DefaultSets,
    int? DefaultRepetitions,
    int? TenantId,
    DateTime CreatedOn);

public sealed record CreateExerciseDto(
    string Name,
    int? BodyPartId = null,
    int? DefaultSets = null,
    int? DefaultRepetitions = null,
    int? TenantId = null);

public sealed record UpdateExerciseDto(
    int ExerciseId,
    string Name,
    int? BodyPartId,
    int? DefaultSets,
    int? DefaultRepetitions);
