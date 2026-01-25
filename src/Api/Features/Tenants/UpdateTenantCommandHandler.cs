using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, TenantDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateTenantCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<TenantDto> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(x => x.Id == request.TenantId, cancellationToken)
            ?? throw new InvalidOperationException($"Tenant with id {request.TenantId} not found.");

        tenant.UpdateName(request.Name, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        return tenant.ToDto();
    }
}
