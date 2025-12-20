using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateScheduledExerciseCommand(
    int TenantId,
    string Name,
    int? DayId,
    int? ExerciseId,
    int Sort,
    string CreatedBy) : IRequest<ScheduledExerciseDto>;
