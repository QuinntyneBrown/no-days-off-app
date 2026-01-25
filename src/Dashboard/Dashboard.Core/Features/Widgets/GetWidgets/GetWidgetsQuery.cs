using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Dashboard;

namespace Dashboard.Core.Features.Widgets.GetWidgets;

public record GetWidgetsQuery(int TenantId, int UserId) : IRequest<IEnumerable<WidgetDto>>;

public class GetWidgetsHandler : IRequestHandler<GetWidgetsQuery, IEnumerable<WidgetDto>>
{
    private readonly IDashboardDbContext _context;

    public GetWidgetsHandler(IDashboardDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WidgetDto>> Handle(GetWidgetsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Widgets
            .Where(w => w.TenantId == request.TenantId && w.UserId == request.UserId && w.IsVisible)
            .OrderBy(w => w.Position)
            .Select(w => new WidgetDto
            {
                WidgetId = w.Id,
                Name = w.Name,
                Type = (int)w.Type,
                TenantId = w.TenantId,
                UserId = w.UserId,
                Position = w.Position,
                Width = w.Width,
                Height = w.Height,
                Configuration = w.Configuration,
                IsVisible = w.IsVisible
            })
            .ToListAsync(cancellationToken);
    }
}
