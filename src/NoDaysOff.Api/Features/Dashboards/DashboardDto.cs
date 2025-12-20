namespace NoDaysOff.Api;

public sealed record DashboardDto(
    int DashboardId,
    string Name,
    string Username,
    bool IsDefault,
    DateTime CreatedOn,
    string CreatedBy);
