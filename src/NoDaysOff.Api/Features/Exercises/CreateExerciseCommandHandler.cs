using MediatR;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.ExerciseAggregate;

namespace NoDaysOff.Api;

public sealed class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, ExerciseDto>
{
    private readonly INoDaysOffContext _context;

    public CreateExerciseCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ExerciseDto> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = Exercise.Create(
            request.TenantId,
            request.Name,
            request.BodyPartId,
            request.DefaultSets,
            request.DefaultRepetitions,
            request.CreatedBy);

        _context.Exercises.Add(exercise);

        await _context.SaveChangesAsync(cancellationToken);

        return exercise.ToDto();
    }
}
