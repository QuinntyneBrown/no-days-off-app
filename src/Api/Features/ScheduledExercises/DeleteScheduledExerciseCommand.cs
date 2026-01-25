using MediatR;

namespace Api;

public sealed record DeleteScheduledExerciseCommand(int ScheduledExerciseId, string DeletedBy) : IRequest;
