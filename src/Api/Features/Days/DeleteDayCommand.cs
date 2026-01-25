using MediatR;

namespace Api;

public sealed record DeleteDayCommand(int DayId, string DeletedBy) : IRequest;
