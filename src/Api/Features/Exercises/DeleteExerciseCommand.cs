using MediatR;

namespace Api;

public sealed record DeleteExerciseCommand(int ExerciseId, string DeletedBy) : IRequest;
