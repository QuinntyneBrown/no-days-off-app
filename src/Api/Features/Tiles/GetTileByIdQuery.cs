using MediatR;

namespace Api;

public sealed record GetTileByIdQuery(int TileId) : IRequest<TileDto?>;
