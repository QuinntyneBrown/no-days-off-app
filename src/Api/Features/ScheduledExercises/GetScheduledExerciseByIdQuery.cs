using MediatR;

namespace Api;

public sealed record GetScheduledExerciseByIdQuery(int ScheduledExerciseId) : IRequest<ScheduledExerciseDto?>;
