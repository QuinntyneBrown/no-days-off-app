using MediatR;

namespace Api;

public sealed record GetDaysQuery : IRequest<IEnumerable<DayDto>>;
