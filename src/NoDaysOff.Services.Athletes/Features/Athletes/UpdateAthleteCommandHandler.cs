using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class UpdateAthleteCommandHandler : IRequestHandler<UpdateAthleteCommand, AthleteDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateAthleteCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<AthleteDto> Handle(UpdateAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _context.Athletes
            .FirstOrDefaultAsync(x => x.Id == request.AthleteId, cancellationToken)
            ?? throw new InvalidOperationException($"Athlete with id {request.AthleteId} not found.");

        athlete.UpdateName(request.Name, request.ModifiedBy);
        athlete.UpdateUsername(request.Username, request.ModifiedBy);

        await _context.SaveChangesAsync(cancellationToken);

        return athlete.ToDto();
    }
}
