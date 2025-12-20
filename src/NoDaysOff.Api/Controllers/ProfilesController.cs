using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NoDaysOff.Api;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfileDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProfilesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{profileId}")]
    public async Task<ActionResult<ProfileDto>> GetById(int profileId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProfileByIdQuery(profileId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProfileDto>> Create([FromBody] CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { profileId = result.ProfileId }, result);
    }

    [HttpPut("{profileId}")]
    public async Task<ActionResult<ProfileDto>> Update(int profileId, [FromBody] UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        if (profileId != command.ProfileId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{profileId}")]
    public async Task<ActionResult> Delete(int profileId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProfileCommand(profileId, deletedBy), cancellationToken);
        return NoContent();
    }
}
