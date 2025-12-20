using MediatR;

namespace NoDaysOff.Api;

public sealed record GetDaysQuery : IRequest<IEnumerable<DayDto>>;
