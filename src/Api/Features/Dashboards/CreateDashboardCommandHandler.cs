using MediatR;
using Core;
using Core.Model.DashboardAggregate;

namespace Api;

public sealed class CreateDashboardCommandHandler : IRequestHandler<CreateDashboardCommand, DashboardDto>
{
    private readonly INoDaysOffContext _context;

    public CreateDashboardCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> Handle(CreateDashboardCommand request, CancellationToken cancellationToken)
    {
        var dashboard = Dashboard.Create(
            request.TenantId,
            request.Name,
            request.Username,
            request.IsDefault,
            request.CreatedBy);

        _context.Dashboards.Add(dashboard);

        await _context.SaveChangesAsync(cancellationToken);

        return dashboard.ToDto();
    }
}
