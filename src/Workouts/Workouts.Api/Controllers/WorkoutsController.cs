using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Contracts.Workouts;
using Workouts.Core.Features.Workouts.CreateWorkout;
using Workouts.Core.Features.Workouts.GetWorkouts;

namespace Workouts.Api.Controllers;

[ApiController]
[Route("workouts")]
[Authorize]
public class WorkoutsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public WorkoutsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutDto>>> GetAll([FromQuery] int? tenantId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWorkoutsQuery(tenantId ?? _currentUser.TenantId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutDto>> Create([FromBody] CreateWorkoutDto request, CancellationToken ct)
    {
        var command = new CreateWorkoutCommand(request.Name, request.TenantId ?? _currentUser.TenantId, _currentUser.Email ?? "system");
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), result);
    }
}
