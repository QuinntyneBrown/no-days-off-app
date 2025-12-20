using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class UpdateScheduledExerciseCommandHandler : IRequestHandler<UpdateScheduledExerciseCommand, ScheduledExerciseDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateScheduledExerciseCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<ScheduledExerciseDto> Handle(UpdateScheduledExerciseCommand request, CancellationToken cancellationToken)
    {
        var scheduledExercise = await _context.ScheduledExercises
            .FirstOrDefaultAsync(x => x.Id == request.ScheduledExerciseId, cancellationToken)
            ?? throw new InvalidOperationException($"ScheduledExercise with id {request.ScheduledExerciseId} not found.");

        scheduledExercise.UpdateName(request.Name, request.ModifiedBy);
        scheduledExercise.UpdateSchedule(request.DayId, request.Sort, request.ModifiedBy);
        scheduledExercise.UpdateExercise(request.ExerciseId, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        return scheduledExercise.ToDto();
    }
}
