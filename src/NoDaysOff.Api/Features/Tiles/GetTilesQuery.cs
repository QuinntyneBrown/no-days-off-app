using MediatR;

namespace NoDaysOff.Api;

public sealed record GetTilesQuery : IRequest<IEnumerable<TileDto>>;
