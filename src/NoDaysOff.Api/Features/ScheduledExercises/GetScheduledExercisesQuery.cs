using MediatR;

namespace NoDaysOff.Api;

public sealed record GetScheduledExercisesQuery : IRequest<IEnumerable<ScheduledExerciseDto>>;
