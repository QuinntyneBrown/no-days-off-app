using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Athletes;
using Shared.Domain.Exceptions;
using Shared.Messaging;
using Shared.Messaging.Messages.Athletes;

namespace Athletes.Core.Features.Athletes.UpdateAthlete;

public sealed class UpdateAthleteCommandHandler : IRequestHandler<UpdateAthleteCommand, AthleteDto>
{
    private readonly IAthletesDbContext _context;
    private readonly IMessageBus _messageBus;

    public UpdateAthleteCommandHandler(IAthletesDbContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<AthleteDto> Handle(UpdateAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _context.Athletes
            .FirstOrDefaultAsync(a => a.Id == request.AthleteId && !a.IsDeleted, cancellationToken);

        if (athlete == null)
            throw NotFoundException.For<Aggregates.Athlete.Athlete>(request.AthleteId);

        athlete.UpdateName(request.Name, request.ModifiedBy);
        athlete.UpdateUsername(request.Username, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(new AthleteUpdatedMessage
        {
            AthleteId = athlete.Id,
            Name = athlete.Name,
            Username = athlete.Username,
            TenantId = athlete.TenantId
        }, MessageTopics.Athletes.AthleteUpdated, cancellationToken);

        return athlete.ToDto();
    }
}
