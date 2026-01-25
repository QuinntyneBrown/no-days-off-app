using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Contracts.Communication;
using Communication.Core.Features.CreateNotification;
using Communication.Core.Features.GetNotifications;
using Communication.Core.Features.MarkAllAsRead;
using Communication.Core.Features.MarkAsRead;

namespace Communication.Api.Controllers;

[ApiController]
[Route("notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public NotificationsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    private int GetTenantId() =>
        _currentUser.TenantId ?? throw new UnauthorizedAccessException("Tenant not specified");

    private int GetUserId() =>
        _currentUser.UserId ?? throw new UnauthorizedAccessException("User not specified");

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAll(
        [FromQuery] bool unreadOnly = false,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetNotificationsQuery(GetTenantId(), GetUserId(), unreadOnly), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<NotificationDto>> Create([FromBody] CreateNotificationDto request, CancellationToken ct)
    {
        var command = new CreateNotificationCommand(
            request.Title,
            request.Message,
            request.Type,
            GetTenantId(),
            request.UserId ?? GetUserId(),
            request.ActionUrl,
            request.EntityType,
            request.EntityId);

        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), result);
    }

    [HttpPost("{id}/read")]
    public async Task<ActionResult> MarkAsRead(int id, CancellationToken ct)
    {
        await _mediator.Send(new MarkAsReadCommand(id, GetTenantId(), GetUserId()), ct);
        return NoContent();
    }

    [HttpPost("read-all")]
    public async Task<ActionResult<int>> MarkAllAsRead(CancellationToken ct)
    {
        var count = await _mediator.Send(new MarkAllAsReadCommand(GetTenantId(), GetUserId()), ct);
        return Ok(new { markedAsRead = count });
    }
}
