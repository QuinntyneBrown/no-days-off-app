using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetAthletesQueryHandler : IRequestHandler<GetAthletesQuery, IEnumerable<AthleteDto>>
{
    private readonly INoDaysOffContext _context;

    public GetAthletesQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AthleteDto>> Handle(GetAthletesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Athletes
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
