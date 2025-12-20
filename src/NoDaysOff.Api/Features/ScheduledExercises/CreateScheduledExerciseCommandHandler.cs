using MediatR;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.ScheduledExerciseAggregate;

namespace NoDaysOff.Api;

public sealed class CreateScheduledExerciseCommandHandler : IRequestHandler<CreateScheduledExerciseCommand, ScheduledExerciseDto>
{
    private readonly INoDaysOffContext _context;

    public CreateScheduledExerciseCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ScheduledExerciseDto> Handle(CreateScheduledExerciseCommand request, CancellationToken cancellationToken)
    {
        var scheduledExercise = ScheduledExercise.Create(
            request.TenantId,
            request.Name,
            request.DayId,
            request.ExerciseId,
            request.Sort,
            request.CreatedBy);

        _context.ScheduledExercises.Add(scheduledExercise);

        await _context.SaveChangesAsync(cancellationToken);

        return scheduledExercise.ToDto();
    }
}
