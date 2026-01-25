using MediatR;
using Core;
using Core.Model.WorkoutAggregate;

namespace Api;

public sealed class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, WorkoutDto>
{
    private readonly INoDaysOffContext _context;

    public CreateWorkoutCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<WorkoutDto> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workout = Workout.Create(request.TenantId, request.CreatedBy);

        _context.Workouts.Add(workout);

        await _context.SaveChangesAsync(cancellationToken);

        return workout.ToDto();
    }
}
