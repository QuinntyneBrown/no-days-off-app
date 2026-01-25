using MediatR;
using Shared.Contracts.Exercises;
using Shared.Messaging;
using Shared.Messaging.Messages.Exercises;
using Exercises.Core.Aggregates.Exercise;

namespace Exercises.Core.Features.Exercises.CreateExercise;

public record CreateExerciseCommand(
    string Name,
    int TenantId,
    int? BodyPartId = null,
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
            ExerciseType.Strength,
            request.BodyPartId);

        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(
            new ExerciseCreatedMessage
            {
                ExerciseId = exercise.Id,
                Name = exercise.Name,
                BodyPartId = exercise.BodyPartId,
                TenantId = exercise.TenantId
            },
            MessageTopics.Exercises.ExerciseCreated,
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
