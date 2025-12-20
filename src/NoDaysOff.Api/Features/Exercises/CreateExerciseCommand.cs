using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateExerciseCommand(
    int TenantId,
    string Name,
    int? BodyPartId,
    int DefaultSets,
    int DefaultRepetitions,
    string CreatedBy) : IRequest<ExerciseDto>;
