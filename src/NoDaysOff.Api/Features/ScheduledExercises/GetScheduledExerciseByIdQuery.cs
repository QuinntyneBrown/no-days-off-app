using MediatR;

namespace NoDaysOff.Api;

public sealed record GetScheduledExerciseByIdQuery(int ScheduledExerciseId) : IRequest<ScheduledExerciseDto?>;
