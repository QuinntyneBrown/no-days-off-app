using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NoDaysOff.Api;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExercisesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetExercisesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{exerciseId}")]
    public async Task<ActionResult<ExerciseDto>> GetById(int exerciseId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetExerciseByIdQuery(exerciseId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseDto>> Create([FromBody] CreateExerciseCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { exerciseId = result.ExerciseId }, result);
    }

    [HttpPut("{exerciseId}")]
    public async Task<ActionResult<ExerciseDto>> Update(int exerciseId, [FromBody] UpdateExerciseCommand command, CancellationToken cancellationToken)
    {
        if (exerciseId != command.ExerciseId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{exerciseId}")]
    public async Task<ActionResult> Delete(int exerciseId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteExerciseCommand(exerciseId, deletedBy), cancellationToken);
        return NoContent();
    }
}
