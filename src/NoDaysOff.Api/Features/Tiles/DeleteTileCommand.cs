using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteTileCommand(int TileId, string DeletedBy) : IRequest;
