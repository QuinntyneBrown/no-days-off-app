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

    [HttpGet]
    public async Task<ActionResult<DashboardStatsDto>> Get(CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetDashboardStatsQuery(_currentUser.TenantId, _currentUser.UserId), ct);

        if (result == null)
            return Ok(new DashboardStatsDto
            {
                TenantId = _currentUser.TenantId,
                UserId = _currentUser.UserId,
                LastUpdated = DateTime.UtcNow
            });

        return Ok(result);
    }
}
