using MediatR;

namespace Api;

public sealed record CreateScheduledExerciseCommand(
    int TenantId,
    string Name,
    int? DayId,
    int? ExerciseId,
    int Sort,
    string CreatedBy) : IRequest<ScheduledExerciseDto>;
