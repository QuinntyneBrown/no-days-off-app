using MediatR;
using Shared.Contracts.Workouts;
using Shared.Messaging;
using Shared.Messaging.Messages.Workouts;
using Workouts.Core.Aggregates.Workout;

namespace Workouts.Core.Features.Workouts.CreateWorkout;

public sealed class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, WorkoutDto>
{
    private readonly IWorkoutsDbContext _context;
    private readonly IMessageBus _messageBus;

    public CreateWorkoutCommandHandler(IWorkoutsDbContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<WorkoutDto> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workout = Workout.Create(request.TenantId, request.Name, request.CreatedBy);

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(new WorkoutCreatedMessage
        {
            WorkoutId = workout.Id,
            Name = workout.Name,
            TenantId = workout.TenantId
        }, MessageTopics.Workouts.WorkoutCreated, cancellationToken);

        return workout.ToDto();
    }
}
