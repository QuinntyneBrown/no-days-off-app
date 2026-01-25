namespace Shared.Contracts.Identity;

public sealed record UserDto(
    int UserId,
    string Email,
    string FirstName,
    string LastName,
    int? TenantId,
    DateTime CreatedOn,
    IEnumerable<string> Roles);
