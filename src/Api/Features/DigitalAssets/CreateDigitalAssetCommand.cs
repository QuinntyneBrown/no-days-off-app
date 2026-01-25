using MediatR;

namespace Api;

public sealed record CreateDigitalAssetCommand(
    string Name,
    string FileName,
    string ContentType,
    long Size,
    string CreatedBy) : IRequest<DigitalAssetDto>;
