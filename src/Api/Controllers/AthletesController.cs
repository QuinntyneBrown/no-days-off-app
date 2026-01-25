using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api;

[ApiController]
[Route("api/[controller]")]
public class AthletesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AthletesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AthleteDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAthletesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{athleteId}")]
    public async Task<ActionResult<AthleteDto>> GetById(int athleteId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAthleteByIdQuery(athleteId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AthleteDto>> Create([FromBody] CreateAthleteCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { athleteId = result.AthleteId }, result);
    }

    [HttpPut("{athleteId}")]
    public async Task<ActionResult<AthleteDto>> Update(int athleteId, [FromBody] UpdateAthleteCommand command, CancellationToken cancellationToken)
    {
        if (athleteId != command.AthleteId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{athleteId}")]
    public async Task<ActionResult> Delete(int athleteId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAthleteCommand(athleteId, deletedBy), cancellationToken);
        return NoContent();
    }
}
