using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NoDaysOff.Api;

[ApiController]
[Route("api/[controller]")]
public class BodyPartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BodyPartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BodyPartDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBodyPartsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{bodyPartId}")]
    public async Task<ActionResult<BodyPartDto>> GetById(int bodyPartId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBodyPartByIdQuery(bodyPartId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BodyPartDto>> Create([FromBody] CreateBodyPartCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { bodyPartId = result.BodyPartId }, result);
    }

    [HttpPut("{bodyPartId}")]
    public async Task<ActionResult<BodyPartDto>> Update(int bodyPartId, [FromBody] UpdateBodyPartCommand command, CancellationToken cancellationToken)
    {
        if (bodyPartId != command.BodyPartId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{bodyPartId}")]
    public async Task<ActionResult> Delete(int bodyPartId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteBodyPartCommand(bodyPartId, deletedBy), cancellationToken);
        return NoContent();
    }
}
