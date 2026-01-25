using MediatR;

namespace Api;

public sealed record GetDayByIdQuery(int DayId) : IRequest<DayDto?>;
