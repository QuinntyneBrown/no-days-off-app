using MediatR;

namespace NoDaysOff.Api;

public sealed record GetDigitalAssetsQuery : IRequest<IEnumerable<DigitalAssetDto>>;
