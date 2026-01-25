using Athletes.Core.Features.Athletes.CreateAthlete;
using Athletes.Core.Features.Athletes.DeleteAthlete;
using Athletes.Core.Features.Athletes.GetAthleteById;
using Athletes.Core.Features.Athletes.GetAthletes;
using Athletes.Core.Features.Athletes.UpdateAthlete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Contracts.Athletes;

namespace Athletes.Api.Controllers;

[ApiController]
[Route("athletes")]
[Authorize]
public class AthletesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public AthletesController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AthleteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AthleteDto>>> GetAll(
        [FromQuery] int? tenantId,
        CancellationToken cancellationToken)
    {
        var query = new GetAthletesQuery(tenantId ?? _currentUser.TenantId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{athleteId:int}")]
    [ProducesResponseType(typeof(AthleteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AthleteDto>> GetById(
        int athleteId,
        CancellationToken cancellationToken)
    {
        var query = new GetAthleteByIdQuery(athleteId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AthleteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AthleteDto>> Create(
        [FromBody] CreateAthleteDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateAthleteCommand(
            request.Name,
            request.Username,
            request.TenantId ?? _currentUser.TenantId,
            _currentUser.Email ?? "system");

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { athleteId = result.AthleteId }, result);
    }

    [HttpPut("{athleteId:int}")]
    [ProducesResponseType(typeof(AthleteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AthleteDto>> Update(
        int athleteId,
        [FromBody] UpdateAthleteDto request,
        CancellationToken cancellationToken)
    {
        if (athleteId != request.AthleteId)
            return BadRequest("ID mismatch");

        var command = new UpdateAthleteCommand(
            request.AthleteId,
            request.Name,
            request.Username,
            _currentUser.Email ?? "system");

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{athleteId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(
        int athleteId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAthleteCommand(athleteId, _currentUser.Email ?? "system");
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
