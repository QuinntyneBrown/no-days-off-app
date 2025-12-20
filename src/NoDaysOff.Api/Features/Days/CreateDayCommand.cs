using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateDayCommand(
    int TenantId,
    string Name,
    string CreatedBy) : IRequest<DayDto>;
