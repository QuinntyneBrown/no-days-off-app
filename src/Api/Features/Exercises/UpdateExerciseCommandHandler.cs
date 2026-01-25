using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, ExerciseDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateExerciseCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ExerciseDto> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises
            .FirstOrDefaultAsync(x => x.Id == request.ExerciseId, cancellationToken)
            ?? throw new InvalidOperationException($"Exercise with id {request.ExerciseId} not found.");

        exercise.UpdateName(request.Name, request.ModifiedBy);
        exercise.UpdateBodyPart(request.BodyPartId, request.ModifiedBy);
        exercise.UpdateDefaults(request.DefaultSets, request.DefaultRepetitions, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        return exercise.ToDto();
    }
}
