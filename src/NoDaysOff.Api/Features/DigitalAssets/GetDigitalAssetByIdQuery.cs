using MediatR;

namespace NoDaysOff.Api;

public sealed record GetDigitalAssetByIdQuery(int DigitalAssetId) : IRequest<DigitalAssetDto?>;
