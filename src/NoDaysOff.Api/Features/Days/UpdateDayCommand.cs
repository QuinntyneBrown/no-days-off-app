using MediatR;

namespace NoDaysOff.Api;

public sealed record UpdateDayCommand(
    int DayId,
    string Name,
    string ModifiedBy) : IRequest<DayDto>;
