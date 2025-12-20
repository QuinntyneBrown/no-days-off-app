using MediatR;

namespace NoDaysOff.Api;

public sealed record GetAthletesQuery : IRequest<IEnumerable<AthleteDto>>;
