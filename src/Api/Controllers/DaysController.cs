using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api;

[ApiController]
[Route("api/[controller]")]
public class DaysController : ControllerBase
{
    private readonly IMediator _mediator;

    public DaysController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DayDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDaysQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{dayId}")]
    public async Task<ActionResult<DayDto>> GetById(int dayId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDayByIdQuery(dayId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DayDto>> Create([FromBody] CreateDayCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { dayId = result.DayId }, result);
    }

    [HttpPut("{dayId}")]
    public async Task<ActionResult<DayDto>> Update(int dayId, [FromBody] UpdateDayCommand command, CancellationToken cancellationToken)
    {
        if (dayId != command.DayId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{dayId}")]
    public async Task<ActionResult> Delete(int dayId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteDayCommand(dayId, deletedBy), cancellationToken);
        return NoContent();
    }
}
