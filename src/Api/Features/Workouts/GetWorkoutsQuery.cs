using MediatR;

namespace Api;

public sealed record GetWorkoutsQuery : IRequest<IEnumerable<WorkoutDto>>;
