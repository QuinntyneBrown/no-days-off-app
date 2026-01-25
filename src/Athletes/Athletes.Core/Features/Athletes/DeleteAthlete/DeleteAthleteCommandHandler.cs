using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;
using Shared.Messaging;
using Shared.Messaging.Messages.Athletes;

namespace Athletes.Core.Features.Athletes.DeleteAthlete;

public sealed class DeleteAthleteCommandHandler : IRequestHandler<DeleteAthleteCommand, bool>
{
    private readonly IAthletesDbContext _context;
    private readonly IMessageBus _messageBus;

    public DeleteAthleteCommandHandler(IAthletesDbContext context, IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<bool> Handle(DeleteAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _context.Athletes
            .FirstOrDefaultAsync(a => a.Id == request.AthleteId && !a.IsDeleted, cancellationToken);

        if (athlete == null)
            throw NotFoundException.For<Aggregates.Athlete.Athlete>(request.AthleteId);

        athlete.Delete();
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(new AthleteDeletedMessage
        {
            AthleteId = athlete.Id,
            TenantId = athlete.TenantId,
            DeletedBy = request.DeletedBy
        }, MessageTopics.Athletes.AthleteDeleted, cancellationToken);

        return true;
    }
}
