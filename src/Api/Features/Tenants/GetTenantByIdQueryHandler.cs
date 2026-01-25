using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, TenantDto?>
{
    private readonly INoDaysOffContext _context;

    public GetTenantByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<TenantDto?> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.TenantId && !x.IsDeleted, cancellationToken);

        return tenant?.ToDto();
    }
}
