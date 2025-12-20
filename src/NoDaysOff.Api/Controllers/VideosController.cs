using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NoDaysOff.Api;

[ApiController]
[Route("api/[controller]")]
public class VideosController : ControllerBase
{
    private readonly IMediator _mediator;

    public VideosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VideoDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVideosQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{videoId}")]
    public async Task<ActionResult<VideoDto>> GetById(int videoId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVideoByIdQuery(videoId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<VideoDto>> Create([FromBody] CreateVideoCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { videoId = result.VideoId }, result);
    }

    [HttpPut("{videoId}")]
    public async Task<ActionResult<VideoDto>> Update(int videoId, [FromBody] UpdateVideoCommand command, CancellationToken cancellationToken)
    {
        if (videoId != command.VideoId)
            return BadRequest();

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{videoId}")]
    public async Task<ActionResult> Delete(int videoId, [FromQuery] string deletedBy, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteVideoCommand(videoId, deletedBy), cancellationToken);
        return NoContent();
    }
}
