using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class DeleteAthleteCommandHandler : IRequestHandler<DeleteAthleteCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteAthleteCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _context.Athletes
            .FirstOrDefaultAsync(x => x.Id == request.AthleteId, cancellationToken)
            ?? throw new InvalidOperationException($"Athlete with id {request.AthleteId} not found.");

        athlete.Delete(request.DeletedBy);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
