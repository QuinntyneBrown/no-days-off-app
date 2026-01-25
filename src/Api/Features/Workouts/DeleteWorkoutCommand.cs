using MediatR;

namespace Api;

public sealed record DeleteWorkoutCommand(int WorkoutId, string DeletedBy) : IRequest;
