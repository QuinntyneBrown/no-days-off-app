using MediatR;

namespace Api;

public sealed record DeleteTileCommand(int TileId, string DeletedBy) : IRequest;
