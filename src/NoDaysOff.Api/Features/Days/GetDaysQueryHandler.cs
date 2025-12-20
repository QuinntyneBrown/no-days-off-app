using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetDaysQueryHandler : IRequestHandler<GetDaysQuery, IEnumerable<DayDto>>
{
    private readonly INoDaysOffContext _context;

    public GetDaysQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DayDto>> Handle(GetDaysQuery request, CancellationToken cancellationToken)
    {
        return await _context.Days
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
