using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Exercises;
using Shared.Domain.Exceptions;
using Shared.Messaging;
using Shared.Messaging.Messages.Exercises;
using Exercises.Core.Aggregates.Exercise;

namespace Exercises.Core.Features.Exercises.UpdateExercise;

public record UpdateExerciseCommand(
    int ExerciseId,
    string Name,
    int TenantId,
    int? BodyPartId = null) : IRequest<ExerciseDto>;

public class UpdateExerciseHandler : IRequestHandler<UpdateExerciseCommand, ExerciseDto>
{
    private readonly IExercisesDbContext _context;
    private readonly IMessageBus _messageBus;

    public UpdateExerciseHandler(IExercisesDbContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<ExerciseDto> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises
            .Where(e => e.Id == request.ExerciseId && e.TenantId == request.TenantId)
            .FirstOrDefaultAsync(cancellationToken);

        if (exercise == null)
            throw new NotFoundException(nameof(Exercise), request.ExerciseId);

        exercise.Update(
            request.Name,
            exercise.Type,
            request.BodyPartId,
            exercise.Description,
            exercise.VideoUrl,
            exercise.ImageUrl,
            exercise.Instructions);

        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(
            new ExerciseUpdatedMessage
            {
                ExerciseId = exercise.Id,
                Name = exercise.Name,
                BodyPartId = exercise.BodyPartId,
                TenantId = exercise.TenantId
            },
            MessageTopics.Exercises.ExerciseUpdated,
            cancellationToken);

        return new ExerciseDto(
            exercise.Id,
            exercise.Name,
            exercise.BodyPartId,
            null,
            null,
            exercise.TenantId,
            exercise.CreatedOn
        );
    }
}
