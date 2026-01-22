using MediatR;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.AthleteAggregate;
using NoDaysOff.Shared.Messages;
using NoDaysOff.Shared.Messaging;

namespace NoDaysOff.Api;

public sealed class CreateAthleteCommandHandler : IRequestHandler<CreateAthleteCommand, AthleteDto>
{
    private readonly INoDaysOffContext _context;
    private readonly IMessageBus _messageBus;

    public CreateAthleteCommandHandler(INoDaysOffContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<AthleteDto> Handle(CreateAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = Athlete.Create(request.TenantId, request.Name, request.Username, request.CreatedBy);

        _context.Athletes.Add(athlete);

        await _context.SaveChangesAsync(cancellationToken);

        // Publish event for other microservices
        await _messageBus.PublishAsync(new AthleteCreatedMessage
        {
            AthleteId = athlete.Id,
            Name = athlete.Name,
            Username = athlete.Username,
            TenantId = athlete.TenantId
        }, "athlete.created", cancellationToken);

        return athlete.ToDto();
    }
}
