using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class GetDigitalAssetsQueryHandler : IRequestHandler<GetDigitalAssetsQuery, IEnumerable<DigitalAssetDto>>
{
    private readonly INoDaysOffContext _context;

    public GetDigitalAssetsQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DigitalAssetDto>> Handle(GetDigitalAssetsQuery request, CancellationToken cancellationToken)
    {
        return await _context.DigitalAssets
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
