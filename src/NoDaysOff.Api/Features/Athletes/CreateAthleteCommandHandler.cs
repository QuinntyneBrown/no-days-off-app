using MediatR;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.AthleteAggregate;

namespace NoDaysOff.Api;

public sealed class CreateAthleteCommandHandler : IRequestHandler<CreateAthleteCommand, AthleteDto>
{
    private readonly INoDaysOffContext _context;

    public CreateAthleteCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<AthleteDto> Handle(CreateAthleteCommand request, CancellationToken cancellationToken)
    {
        var athlete = Athlete.Create(request.TenantId, request.Name, request.Username, request.CreatedBy);

        _context.Athletes.Add(athlete);

        await _context.SaveChangesAsync(cancellationToken);

        return athlete.ToDto();
    }
}
