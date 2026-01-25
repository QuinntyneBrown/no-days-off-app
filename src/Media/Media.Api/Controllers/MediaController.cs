using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Contracts.Media;
using Media.Core.Features.DeleteMediaFile;
using Media.Core.Features.GetMediaFiles;
using Media.Core.Features.UploadMediaFile;

namespace Media.Api.Controllers;

[ApiController]
[Route("media")]
[Authorize]
public class MediaController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public MediaController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaFileDto>>> GetAll(
        [FromQuery] int? entityId,
        [FromQuery] string? entityType,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetMediaFilesQuery(_currentUser.TenantId, entityId, entityType), ct);
        return Ok(result);
    }

    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    public async Task<ActionResult<MediaFileDto>> Upload(IFormFile file, [FromQuery] int? entityId, [FromQuery] string? entityType, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        using var stream = file.OpenReadStream();
        var command = new UploadMediaFileCommand(
            stream,
            file.FileName,
            file.ContentType,
            file.Length,
            _currentUser.TenantId,
            _currentUser.Email ?? "system",
            entityId,
            entityType);

        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteMediaFileCommand(id, _currentUser.TenantId), ct);
        return NoContent();
    }
}
