using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteExerciseCommand(int ExerciseId, string DeletedBy) : IRequest;
