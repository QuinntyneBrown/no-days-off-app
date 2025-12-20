using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteWorkoutCommand(int WorkoutId, string DeletedBy) : IRequest;
