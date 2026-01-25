using MediatR;
using Shared.Contracts.Dashboard;
using Dashboard.Core.Aggregates.Widget;

namespace Dashboard.Core.Features.Widgets.CreateWidget;

public record CreateWidgetCommand(
    string Name,
    int Type,
    int TenantId,
    int UserId,
    int Position = 0,
    int Width = 1,
    int Height = 1,
    string? Configuration = null) : IRequest<WidgetDto>;

public class CreateWidgetHandler : IRequestHandler<CreateWidgetCommand, WidgetDto>
{
    private readonly IDashboardDbContext _context;

    public CreateWidgetHandler(IDashboardDbContext context)
    {
        _context = context;
    }

    public async Task<WidgetDto> Handle(CreateWidgetCommand request, CancellationToken cancellationToken)
    {
        var widget = new Widget(
            request.Name,
            (WidgetType)request.Type,
            request.TenantId,
            request.UserId,
            request.Position,
            request.Width,
            request.Height,
            request.Configuration);

        _context.Widgets.Add(widget);
        await _context.SaveChangesAsync(cancellationToken);

        return new WidgetDto
        {
            WidgetId = widget.Id,
            Name = widget.Name,
            Type = (int)widget.Type,
            TenantId = widget.TenantId,
            UserId = widget.UserId,
            Position = widget.Position,
            Width = widget.Width,
            Height = widget.Height,
            Configuration = widget.Configuration,
            IsVisible = widget.IsVisible
        };
    }
}
