using MediatR;

namespace Api;

public sealed record UpdateScheduledExerciseCommand(
    int ScheduledExerciseId,
    string Name,
    int? DayId,
    int? ExerciseId,
    int Sort,
    string ModifiedBy) : IRequest<ScheduledExerciseDto>;
