using MediatR;

namespace NoDaysOff.Api;

public sealed record GetWorkoutsQuery : IRequest<IEnumerable<WorkoutDto>>;
