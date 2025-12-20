using MediatR;

namespace NoDaysOff.Api;

public sealed record GetDayByIdQuery(int DayId) : IRequest<DayDto?>;
