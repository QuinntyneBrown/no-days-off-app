using Athletes.Core.Aggregates.Athlete;
using MediatR;
using Shared.Contracts.Athletes;
using Shared.Messaging;
using Shared.Messaging.Messages.Athletes;

namespace Athletes.Core.Features.Athletes.CreateAthlete;

public sealed class CreateAthleteCommandHandler : IRequestHandler<CreateAthleteCommand, AthleteDto>
{
    private readonly IAthletesDbContext _context;
    private readonly IMessageBus _messageBus;

    public CreateAthleteCommandHandler(IAthletesDbContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<AthleteDto> Handle(CreateAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = Athlete.Create(
            request.TenantId,
            request.Name,
            request.Username,
            request.CreatedBy);

        _context.Athletes.Add(athlete);
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(new AthleteCreatedMessage
        {
            AthleteId = athlete.Id,
            Name = athlete.Name,
            Username = athlete.Username,
            TenantId = athlete.TenantId
        }, MessageTopics.Athletes.AthleteCreated, cancellationToken);

        return athlete.ToDto();
    }
}
