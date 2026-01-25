using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Contracts.Exercises;
using Exercises.Core.Features.BodyParts.CreateBodyPart;
using Exercises.Core.Features.BodyParts.GetBodyParts;

namespace Exercises.Api.Controllers;

[ApiController]
[Route("bodyparts")]
[Authorize]
public class BodyPartsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public BodyPartsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BodyPartDto>>> GetAll([FromQuery] int? tenantId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBodyPartsQuery(tenantId ?? _currentUser.TenantId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BodyPartDto>> Create([FromBody] CreateBodyPartDto request, CancellationToken ct)
    {
        var command = new CreateBodyPartCommand(
            request.Name,
            request.TenantId ?? _currentUser.TenantId,
            request.Description);

        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), result);
    }
}
