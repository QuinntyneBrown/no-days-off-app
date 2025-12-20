using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetDashboardsQueryHandler : IRequestHandler<GetDashboardsQuery, IEnumerable<DashboardDto>>
{
    private readonly INoDaysOffContext _context;

    public GetDashboardsQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DashboardDto>> Handle(GetDashboardsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Dashboards
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
