using MediatR;

namespace NoDaysOff.Api;

public sealed record CreateTileCommand(
    int TenantId,
    string Name,
    int DefaultWidth,
    int DefaultHeight,
    string CreatedBy) : IRequest<TileDto>;
