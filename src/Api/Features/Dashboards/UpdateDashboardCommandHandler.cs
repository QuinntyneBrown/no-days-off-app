using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class UpdateDashboardCommandHandler : IRequestHandler<UpdateDashboardCommand, DashboardDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateDashboardCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> Handle(UpdateDashboardCommand request, CancellationToken cancellationToken)
    {
        var dashboard = await _context.Dashboards
            .FirstOrDefaultAsync(x => x.Id == request.DashboardId, cancellationToken)
            ?? throw new InvalidOperationException($"Dashboard with id {request.DashboardId} not found.");

        dashboard.UpdateName(request.Name, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        return dashboard.ToDto();
    }
}
