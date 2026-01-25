using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class UpdateDayCommandHandler : IRequestHandler<UpdateDayCommand, DayDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateDayCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<DayDto> Handle(UpdateDayCommand request, CancellationToken cancellationToken)
    {
        var day = await _context.Days
            .FirstOrDefaultAsync(x => x.Id == request.DayId, cancellationToken)
            ?? throw new InvalidOperationException($"Day with id {request.DayId} not found.");

        day.UpdateName(request.Name, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        return day.ToDto();
    }
}
