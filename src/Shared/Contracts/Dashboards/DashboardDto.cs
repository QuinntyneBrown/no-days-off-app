namespace Shared.Contracts.Dashboards;

public sealed record DashboardDto(
    int DashboardId,
    string Name,
    string Username,
    bool IsDefault,
    IEnumerable<DashboardTileDto> Tiles,
    int? TenantId,
    DateTime CreatedOn);

public sealed record DashboardTileDto(
    int DashboardTileId,
    int TileId,
    int Row,
    int Column,
    int Width,
    int Height);

public sealed record CreateDashboardDto(
    string Name,
    string Username,
    bool IsDefault = false,
    int? TenantId = null);

public sealed record UpdateDashboardDto(
    int DashboardId,
    string Name,
    bool IsDefault);
