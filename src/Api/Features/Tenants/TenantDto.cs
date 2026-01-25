namespace Api;

public sealed record TenantDto(
    int TenantId,
    Guid UniqueId,
    string Name,
    DateTime CreatedOn,
    string CreatedBy);
