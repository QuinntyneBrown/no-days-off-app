using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Contracts.Exercises;
using Exercises.Core.Features.Exercises.CreateExercise;
using Exercises.Core.Features.Exercises.DeleteExercise;
using Exercises.Core.Features.Exercises.GetExerciseById;
using Exercises.Core.Features.Exercises.GetExercises;
using Exercises.Core.Features.Exercises.UpdateExercise;

namespace Exercises.Api.Controllers;

[ApiController]
[Route("exercises")]
[Authorize]
public class ExercisesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public ExercisesController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    private int GetTenantId(int? requestTenantId) =>
        requestTenantId ?? _currentUser.TenantId ?? throw new UnauthorizedAccessException("Tenant not specified");

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAll(
        [FromQuery] int? tenantId,
        [FromQuery] int? bodyPartId,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetExercisesQuery(GetTenantId(tenantId), bodyPartId), ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExerciseDto>> GetById(int id, [FromQuery] int? tenantId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetExerciseByIdQuery(id, GetTenantId(tenantId)), ct);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseDto>> Create([FromBody] CreateExerciseDto request, CancellationToken ct)
    {
        var command = new CreateExerciseCommand(
            request.Name,
            GetTenantId(request.TenantId),
            request.BodyPartId,
            _currentUser.Email ?? "system");

        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ExerciseId }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExerciseDto>> Update(int id, [FromBody] UpdateExerciseDto request, CancellationToken ct)
    {
        var command = new UpdateExerciseCommand(
            id,
            request.Name,
            GetTenantId(null),
            request.BodyPartId);

        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id, [FromQuery] int? tenantId, CancellationToken ct)
    {
        await _mediator.Send(new DeleteExerciseCommand(id, GetTenantId(tenantId)), ct);
        return NoContent();
    }
}
