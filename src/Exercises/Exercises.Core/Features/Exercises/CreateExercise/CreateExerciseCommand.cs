using MediatR;
using Shared.Contracts.Exercises;
using Shared.Messaging;
using Shared.Messaging.Messages.Exercises;
using Exercises.Core.Aggregates.Exercise;

namespace Exercises.Core.Features.Exercises.CreateExercise;

public record CreateExerciseCommand(
    string Name,
    int TenantId,
    int Type = 0,
    int? BodyPartId = null,
    string? Description = null,
    string? VideoUrl = null,
    string? ImageUrl = null,
    string? Instructions = null,
    string CreatedBy = "system") : IRequest<ExerciseDto>;

public class CreateExerciseHandler : IRequestHandler<CreateExerciseCommand, ExerciseDto>
{
    private readonly IExercisesDbContext _context;
    private readonly IMessageBus _messageBus;

    public CreateExerciseHandler(IExercisesDbContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<ExerciseDto> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = new Exercise(
            request.Name,
            request.TenantId,
            (ExerciseType)request.Type,
            request.BodyPartId,
            request.Description,
            request.VideoUrl,
            request.ImageUrl,
            request.Instructions);

        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(
            MessageTopics.ExerciseCreated,
            new ExerciseCreatedMessage(exercise.Id, exercise.Name, exercise.TenantId),
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
