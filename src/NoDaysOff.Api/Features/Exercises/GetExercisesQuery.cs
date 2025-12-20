using MediatR;

namespace NoDaysOff.Api;

public sealed record GetExercisesQuery : IRequest<IEnumerable<ExerciseDto>>;
