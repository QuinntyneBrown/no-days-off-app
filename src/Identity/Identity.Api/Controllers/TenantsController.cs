using Identity.Core.Features.Tenants.CreateTenant;
using Identity.Core.Features.Tenants.GetTenantById;
using Identity.Core.Features.Tenants.GetTenants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Authentication;
using Shared.Contracts.Identity;

namespace Identity.Api.Controllers;

[ApiController]
[Route("tenants")]
[Authorize]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public TenantsController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TenantDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetTenantsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{tenantId:int}")]
    [ProducesResponseType(typeof(TenantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TenantDto>> GetById(
        int tenantId,
        CancellationToken cancellationToken)
    {
        var query = new GetTenantByIdQuery(tenantId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TenantDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TenantDto>> Create(
        [FromBody] CreateTenantRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTenantCommand(
            request.Name,
            _currentUser.Email ?? "system");

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { tenantId = result.TenantId }, result);
    }
}

public record CreateTenantRequest(string Name);
