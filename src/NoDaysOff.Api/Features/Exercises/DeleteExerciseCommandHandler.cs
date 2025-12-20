using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteExerciseCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises
            .FirstOrDefaultAsync(x => x.Id == request.ExerciseId, cancellationToken)
            ?? throw new InvalidOperationException($"Exercise with id {request.ExerciseId} not found.");

        exercise.Delete();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
