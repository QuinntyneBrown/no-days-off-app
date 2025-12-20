using MediatR;

namespace NoDaysOff.Api;

public sealed record GetWorkoutByIdQuery(int WorkoutId) : IRequest<WorkoutDto?>;
