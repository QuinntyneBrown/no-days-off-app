using MediatR;

namespace NoDaysOff.Api;

public sealed record UpdateExerciseCommand(
    int ExerciseId,
    string Name,
    int? BodyPartId,
    int DefaultSets,
    int DefaultRepetitions,
    string ModifiedBy) : IRequest<ExerciseDto>;
