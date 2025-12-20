using MediatR;

namespace NoDaysOff.Api;

public sealed record GetExerciseByIdQuery(int ExerciseId) : IRequest<ExerciseDto?>;
