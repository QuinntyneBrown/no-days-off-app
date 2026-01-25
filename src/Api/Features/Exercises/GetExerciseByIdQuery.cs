using MediatR;

namespace Api;

public sealed record GetExerciseByIdQuery(int ExerciseId) : IRequest<ExerciseDto?>;
