using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteTenantCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(x => x.Id == request.TenantId, cancellationToken)
            ?? throw new InvalidOperationException($"Tenant with id {request.TenantId} not found.");

        tenant.Delete();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
