using MediatR;

namespace NoDaysOff.Api;

public sealed record GetTileByIdQuery(int TileId) : IRequest<TileDto?>;
