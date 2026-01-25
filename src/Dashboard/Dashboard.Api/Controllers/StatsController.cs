using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Contracts.Dashboard;
using Dashboard.Core.Features.Stats.GetDashboardStats;

namespace Dashboard.Api.Controllers;

[ApiController]
[Route("stats")]
[Authorize]
public class StatsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public StatsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    private int GetTenantId() =>
        _currentUser.TenantId ?? throw new UnauthorizedAccessException("Tenant not specified");

    private int GetUserId() =>
        _currentUser.UserId ?? throw new UnauthorizedAccessException("User not specified");

    [HttpGet]
    public async Task<ActionResult<DashboardStatsDto>> Get(CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetDashboardStatsQuery(GetTenantId(), GetUserId()), ct);

        if (result == null)
            return Ok(new DashboardStatsDto
            {
                TenantId = GetTenantId(),
                UserId = GetUserId(),
                LastUpdated = DateTime.UtcNow
            });

        return Ok(result);
    }
}
