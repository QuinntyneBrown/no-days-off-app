using MediatR;

namespace Api;

public sealed record GetExercisesQuery : IRequest<IEnumerable<ExerciseDto>>;
