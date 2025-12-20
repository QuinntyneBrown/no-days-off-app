using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteWorkoutCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workout = await _context.Workouts
            .FirstOrDefaultAsync(x => x.Id == request.WorkoutId, cancellationToken)
            ?? throw new InvalidOperationException($"Workout with id {request.WorkoutId} not found.");

        workout.Delete();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
