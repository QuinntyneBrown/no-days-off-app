using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NoDaysOff.Api;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTenantsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{tenantId}")]
    public async Task<ActionResult<TenantDto>> GetById(int tenantId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTenantByIdQuery(tenantId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TenantDto>> Create([FromBody] CreateTenantCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { tenantId = result.TenantId }, result);
    }

    [HttpPut("{tenantId}")]
    public async Task<ActionResult<TenantDto>> Update(int tenantId, [FromBody] UpdateTenantCommand command, CancellationToken cancellationToken)
    {
        if (tenantId != command.TenantId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{tenantId}")]
    public async Task<ActionResult> Delete(int tenantId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTenantCommand(tenantId, deletedBy), cancellationToken);
        return NoContent();
    }
}
