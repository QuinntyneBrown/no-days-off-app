using MediatR;

namespace Api;

public sealed record GetDigitalAssetsQuery : IRequest<IEnumerable<DigitalAssetDto>>;
