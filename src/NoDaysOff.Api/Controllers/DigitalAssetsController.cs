using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NoDaysOff.Api;

[ApiController]
[Route("api/[controller]")]
public class DigitalAssetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DigitalAssetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DigitalAssetDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDigitalAssetsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{digitalAssetId}")]
    public async Task<ActionResult<DigitalAssetDto>> GetById(int digitalAssetId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDigitalAssetByIdQuery(digitalAssetId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DigitalAssetDto>> Create([FromBody] CreateDigitalAssetCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { digitalAssetId = result.DigitalAssetId }, result);
    }

    [HttpPut("{digitalAssetId}")]
    public async Task<ActionResult<DigitalAssetDto>> Update(int digitalAssetId, [FromBody] UpdateDigitalAssetCommand command, CancellationToken cancellationToken)
    {
        if (digitalAssetId != command.DigitalAssetId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{digitalAssetId}")]
    public async Task<ActionResult> Delete(int digitalAssetId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteDigitalAssetCommand(digitalAssetId, deletedBy), cancellationToken);
        return NoContent();
    }
}
