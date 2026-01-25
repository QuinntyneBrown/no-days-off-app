namespace Shared.Authentication;

/// <summary>
/// Service to access current user information from the HTTP context
/// </summary>
public interface ICurrentUserService
{
    int? UserId { get; }
    string? Email { get; }
    int? TenantId { get; }
    bool IsAuthenticated { get; }
    IEnumerable<string> Roles { get; }
    bool IsInRole(string role);
}
