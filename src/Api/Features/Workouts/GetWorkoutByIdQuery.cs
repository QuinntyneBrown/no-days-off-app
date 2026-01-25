using MediatR;

namespace Api;

public sealed record GetWorkoutByIdQuery(int WorkoutId) : IRequest<WorkoutDto?>;
