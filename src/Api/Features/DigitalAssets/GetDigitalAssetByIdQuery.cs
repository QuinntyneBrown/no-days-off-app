using MediatR;

namespace Api;

public sealed record GetDigitalAssetByIdQuery(int DigitalAssetId) : IRequest<DigitalAssetDto?>;
