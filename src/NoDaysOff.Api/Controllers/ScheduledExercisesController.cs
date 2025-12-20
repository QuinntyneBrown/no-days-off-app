using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NoDaysOff.Api;

[ApiController]
[Route("api/[controller]")]
public class ScheduledExercisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduledExercisesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduledExerciseDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetScheduledExercisesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{scheduledExerciseId}")]
    public async Task<ActionResult<ScheduledExerciseDto>> GetById(int scheduledExerciseId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetScheduledExerciseByIdQuery(scheduledExerciseId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ScheduledExerciseDto>> Create([FromBody] CreateScheduledExerciseCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { scheduledExerciseId = result.ScheduledExerciseId }, result);
    }

    [HttpPut("{scheduledExerciseId}")]
    public async Task<ActionResult<ScheduledExerciseDto>> Update(int scheduledExerciseId, [FromBody] UpdateScheduledExerciseCommand command, CancellationToken cancellationToken)
    {
        if (scheduledExerciseId != command.ScheduledExerciseId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{scheduledExerciseId}")]
    public async Task<ActionResult> Delete(int scheduledExerciseId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteScheduledExerciseCommand(scheduledExerciseId, deletedBy), cancellationToken);
        return NoContent();
    }
}
