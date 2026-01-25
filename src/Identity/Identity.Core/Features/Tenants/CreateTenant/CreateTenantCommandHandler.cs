using Identity.Core.Aggregates.Tenant;
using MediatR;
using Shared.Contracts.Identity;
using Shared.Messaging;
using Shared.Messaging.Messages.Identity;

namespace Identity.Core.Features.Tenants.CreateTenant;

public sealed class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, TenantDto>
{
    private readonly IIdentityDbContext _context;
    private readonly IMessageBus _messageBus;

    public CreateTenantCommandHandler(IIdentityDbContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<TenantDto> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = Tenant.Create(request.Name, request.CreatedBy);

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(new TenantCreatedMessage
        {
            TenantId = tenant.Id,
            UniqueId = tenant.UniqueId,
            Name = tenant.Name
        }, MessageTopics.Identity.TenantCreated, cancellationToken);

        return tenant.ToDto();
    }
}
