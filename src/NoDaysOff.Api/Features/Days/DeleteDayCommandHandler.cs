using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteDayCommandHandler : IRequestHandler<DeleteDayCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteDayCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDayCommand request, CancellationToken cancellationToken)
    {
        var day = await _context.Days
            .FirstOrDefaultAsync(x => x.Id == request.DayId, cancellationToken)
            ?? throw new InvalidOperationException($"Day with id {request.DayId} not found.");

        day.Delete(request.DeletedBy);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
