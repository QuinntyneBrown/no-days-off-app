using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Contracts.Dashboard;
using Dashboard.Core.Features.Widgets.CreateWidget;
using Dashboard.Core.Features.Widgets.GetWidgets;

namespace Dashboard.Api.Controllers;

[ApiController]
[Route("widgets")]
[Authorize]
public class WidgetsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public WidgetsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WidgetDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetWidgetsQuery(_currentUser.TenantId, _currentUser.UserId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WidgetDto>> Create([FromBody] CreateWidgetDto request, CancellationToken ct)
    {
        var command = new CreateWidgetCommand(
            request.Name,
            request.Type,
            _currentUser.TenantId,
            _currentUser.UserId,
            request.Position,
            request.Width,
            request.Height,
            request.Configuration);

        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), result);
    }
}
