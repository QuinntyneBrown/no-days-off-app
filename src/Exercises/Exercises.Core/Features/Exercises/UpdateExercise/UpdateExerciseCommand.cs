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
    int Type = 0,
    int? BodyPartId = null,
    string? Description = null,
    string? VideoUrl = null,
    string? ImageUrl = null,
    string? Instructions = null) : IRequest<ExerciseDto>;

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
            (ExerciseType)request.Type,
            request.BodyPartId,
            request.Description,
            request.VideoUrl,
            request.ImageUrl,
            request.Instructions);

        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(
            MessageTopics.ExerciseUpdated,
            new ExerciseUpdatedMessage(exercise.Id, exercise.Name, exercise.TenantId),
            cancellationToken);

        return new ExerciseDto
        {
            ExerciseId = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description,
            TenantId = exercise.TenantId,
            BodyPartId = exercise.BodyPartId,
            VideoUrl = exercise.VideoUrl,
            ImageUrl = exercise.ImageUrl,
            Instructions = exercise.Instructions,
            Type = (int)exercise.Type
        };
    }
}
