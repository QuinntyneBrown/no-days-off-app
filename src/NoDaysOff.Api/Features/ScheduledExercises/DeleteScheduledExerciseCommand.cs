using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteScheduledExerciseCommand(int ScheduledExerciseId, string DeletedBy) : IRequest;
