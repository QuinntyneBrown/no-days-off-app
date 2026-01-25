namespace Shared.Contracts.Identity;

public sealed record TenantDto(
    int TenantId,
    Guid UniqueId,
    string Name,
    DateTime CreatedOn);
