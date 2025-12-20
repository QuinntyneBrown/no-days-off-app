using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteDashboardCommandHandler : IRequestHandler<DeleteDashboardCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteDashboardCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDashboardCommand request, CancellationToken cancellationToken)
    {
        var dashboard = await _context.Dashboards
            .FirstOrDefaultAsync(x => x.Id == request.DashboardId, cancellationToken)
            ?? throw new InvalidOperationException($"Dashboard with id {request.DashboardId} not found.");

        dashboard.Delete();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
