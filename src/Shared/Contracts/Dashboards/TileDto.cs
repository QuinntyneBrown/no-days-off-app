namespace Shared.Contracts.Dashboards;

public sealed record TileDto(
    int TileId,
    string Name,
    string Type,
    string? Configuration,
    int? TenantId,
    DateTime CreatedOn);

public sealed record CreateTileDto(
    string Name,
    string Type,
    string? Configuration = null,
    int? TenantId = null);

public sealed record UpdateTileDto(
    int TileId,
    string Name,
    string Type,
    string? Configuration);
