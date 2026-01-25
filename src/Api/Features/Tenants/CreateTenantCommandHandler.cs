using MediatR;
using Core;
using Core.Model.TenantAggregate;

namespace Api;

public sealed class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, TenantDto>
{
    private readonly INoDaysOffContext _context;

    public CreateTenantCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<TenantDto> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = Tenant.Create(request.Name, request.CreatedBy);

        _context.Tenants.Add(tenant);

        await _context.SaveChangesAsync(cancellationToken);

        return tenant.ToDto();
    }
}
