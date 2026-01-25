using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;
using Shared.Messaging;
using Shared.Messaging.Messages.Exercises;
using Exercises.Core.Aggregates.Exercise;

namespace Exercises.Core.Features.Exercises.DeleteExercise;

public record DeleteExerciseCommand(int ExerciseId, int TenantId, string DeletedBy = "system") : IRequest<bool>;

public class DeleteExerciseHandler : IRequestHandler<DeleteExerciseCommand, bool>
{
    private readonly IExercisesDbContext _context;
    private readonly IMessageBus _messageBus;

    public DeleteExerciseHandler(IExercisesDbContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<bool> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises
            .Where(e => e.Id == request.ExerciseId && e.TenantId == request.TenantId)
            .FirstOrDefaultAsync(cancellationToken);

        if (exercise == null)
            throw new NotFoundException(nameof(Exercise), request.ExerciseId);

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(
            MessageTopics.Exercises.ExerciseDeleted,
            new ExerciseDeletedMessage
            {
                ExerciseId = exercise.Id,
                TenantId = exercise.TenantId,
                DeletedBy = request.DeletedBy
            },
            cancellationToken);

        return true;
    }
}
