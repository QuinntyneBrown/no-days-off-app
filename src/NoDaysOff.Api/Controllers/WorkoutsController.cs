using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NoDaysOff.Api;

[ApiController]
[Route("api/[controller]")]
public class WorkoutsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkoutsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWorkoutsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{workoutId}")]
    public async Task<ActionResult<WorkoutDto>> GetById(int workoutId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWorkoutByIdQuery(workoutId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutDto>> Create([FromBody] CreateWorkoutCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { workoutId = result.WorkoutId }, result);
    }

    [HttpDelete("{workoutId}")]
    public async Task<ActionResult> Delete(int workoutId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteWorkoutCommand(workoutId, deletedBy), cancellationToken);
        return NoContent();
    }
}
