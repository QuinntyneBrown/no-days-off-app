using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shared.Authentication;

/// <summary>
/// Implementation of ICurrentUserService that reads from HTTP context
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public int? UserId
    {
        get
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User?.FindFirst("sub")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value
        ?? User?.FindFirst("email")?.Value;

    public int? TenantId
    {
        get
        {
            var tenantIdClaim = User?.FindFirst("tenant_id")?.Value;
            return int.TryParse(tenantIdClaim, out var tenantId) ? tenantId : null;
        }
    }

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles => User?.FindAll(ClaimTypes.Role)
        .Select(c => c.Value) ?? Enumerable.Empty<string>();

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}
