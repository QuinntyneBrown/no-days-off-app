using MediatR;

namespace Api;

public sealed record DeleteDigitalAssetCommand(int DigitalAssetId, string DeletedBy) : IRequest;
