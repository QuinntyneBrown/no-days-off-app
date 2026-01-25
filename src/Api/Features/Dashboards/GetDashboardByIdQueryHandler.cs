using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class GetDashboardByIdQueryHandler : IRequestHandler<GetDashboardByIdQuery, DashboardDto?>
{
    private readonly INoDaysOffContext _context;

    public GetDashboardByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto?> Handle(GetDashboardByIdQuery request, CancellationToken cancellationToken)
    {
        var dashboard = await _context.Dashboards
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.DashboardId && !x.IsDeleted, cancellationToken);

        return dashboard?.ToDto();
    }
}
