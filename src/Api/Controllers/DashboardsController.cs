using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api;

[ApiController]
[Route("api/[controller]")]
public class DashboardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DashboardDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDashboardsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{dashboardId}")]
    public async Task<ActionResult<DashboardDto>> GetById(int dashboardId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDashboardByIdQuery(dashboardId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DashboardDto>> Create([FromBody] CreateDashboardCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { dashboardId = result.DashboardId }, result);
    }

    [HttpPut("{dashboardId}")]
    public async Task<ActionResult<DashboardDto>> Update(int dashboardId, [FromBody] UpdateDashboardCommand command, CancellationToken cancellationToken)
    {
        if (dashboardId != command.DashboardId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{dashboardId}")]
    public async Task<ActionResult> Delete(int dashboardId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteDashboardCommand(dashboardId, deletedBy), cancellationToken);
        return NoContent();
    }
}
