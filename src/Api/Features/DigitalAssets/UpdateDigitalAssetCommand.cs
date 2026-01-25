using MediatR;

namespace Api;

public sealed record UpdateDigitalAssetCommand(
    int DigitalAssetId,
    string Name,
    string? Description,
    string ModifiedBy) : IRequest<DigitalAssetDto>;
