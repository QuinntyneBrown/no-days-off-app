using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Tenants.GetTenants;

public sealed class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, IEnumerable<TenantDto>>
{
    private readonly IIdentityDbContext _context;

    public GetTenantsQueryHandler(IIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TenantDto>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var tenants = await _context.Tenants
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .ToListAsync(cancellationToken);

        return tenants.Select(t => t.ToDto());
    }
}
