using MediatR;

namespace Api;

public sealed record GetScheduledExercisesQuery : IRequest<IEnumerable<ScheduledExerciseDto>>;
