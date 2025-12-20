using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetDigitalAssetByIdQueryHandler : IRequestHandler<GetDigitalAssetByIdQuery, DigitalAssetDto?>
{
    private readonly INoDaysOffContext _context;

    public GetDigitalAssetByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<DigitalAssetDto?> Handle(GetDigitalAssetByIdQuery request, CancellationToken cancellationToken)
    {
        var digitalAsset = await _context.DigitalAssets
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.DigitalAssetId && !x.IsDeleted, cancellationToken);

        return digitalAsset?.ToDto();
    }
}
