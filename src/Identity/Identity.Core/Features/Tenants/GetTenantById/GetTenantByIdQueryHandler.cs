using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Identity;

namespace Identity.Core.Features.Tenants.GetTenantById;

public sealed class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, TenantDto?>
{
    private readonly IIdentityDbContext _context;

    public GetTenantByIdQueryHandler(IIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<TenantDto?> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.TenantId && !t.IsDeleted, cancellationToken);

        return tenant?.ToDto();
    }
}
