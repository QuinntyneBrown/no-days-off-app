using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteScheduledExerciseCommandHandler : IRequestHandler<DeleteScheduledExerciseCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteScheduledExerciseCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteScheduledExerciseCommand request, CancellationToken cancellationToken)
    {
        var scheduledExercise = await _context.ScheduledExercises
            .FirstOrDefaultAsync(x => x.Id == request.ScheduledExerciseId, cancellationToken)
            ?? throw new InvalidOperationException($"ScheduledExercise with id {request.ScheduledExerciseId} not found.");

        scheduledExercise.Delete(request.DeletedBy);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
