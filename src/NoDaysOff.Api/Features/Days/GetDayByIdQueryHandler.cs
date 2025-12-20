using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetDayByIdQueryHandler : IRequestHandler<GetDayByIdQuery, DayDto?>
{
    private readonly INoDaysOffContext _context;

    public GetDayByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<DayDto?> Handle(GetDayByIdQuery request, CancellationToken cancellationToken)
    {
        var day = await _context.Days
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.DayId && !x.IsDeleted, cancellationToken);

        return day?.ToDto();
    }
}
