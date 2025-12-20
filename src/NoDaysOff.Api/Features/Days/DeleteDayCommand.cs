using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteDayCommand(int DayId, string DeletedBy) : IRequest;
