using MediatR;

namespace NoDaysOff.Api;

public sealed record DeleteDigitalAssetCommand(int DigitalAssetId, string DeletedBy) : IRequest;
