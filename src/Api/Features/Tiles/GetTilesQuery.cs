using MediatR;

namespace Api;

public sealed record GetTilesQuery : IRequest<IEnumerable<TileDto>>;
