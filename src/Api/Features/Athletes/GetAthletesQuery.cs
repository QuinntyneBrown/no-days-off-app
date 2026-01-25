using MediatR;

namespace Api;

public sealed record GetAthletesQuery : IRequest<IEnumerable<AthleteDto>>;
