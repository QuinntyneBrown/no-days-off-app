using MediatR;

namespace NoDaysOff.Api;

public sealed record UpdateTileCommand(
    int TileId,
    string Name,
    int DefaultWidth,
    int DefaultHeight,
    string ModifiedBy) : IRequest<TileDto>;
