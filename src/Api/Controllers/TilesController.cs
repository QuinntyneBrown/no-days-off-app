using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api;

[ApiController]
[Route("api/[controller]")]
public class TilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TileDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTilesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{tileId}")]
    public async Task<ActionResult<TileDto>> GetById(int tileId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTileByIdQuery(tileId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TileDto>> Create([FromBody] CreateTileCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { tileId = result.TileId }, result);
    }

    [HttpPut("{tileId}")]
    public async Task<ActionResult<TileDto>> Update(int tileId, [FromBody] UpdateTileCommand command, CancellationToken cancellationToken)
    {
        if (tileId != command.TileId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{tileId}")]
    public async Task<ActionResult> Delete(int tileId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTileCommand(tileId, deletedBy), cancellationToken);
        return NoContent();
    }
}
