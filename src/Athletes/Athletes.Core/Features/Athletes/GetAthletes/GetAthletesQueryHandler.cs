using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.GetAthletes;

public sealed class GetAthletesQueryHandler : IRequestHandler<GetAthletesQuery, IEnumerable<AthleteDto>>
{
    private readonly IAthletesDbContext _context;

    public GetAthletesQueryHandler(IAthletesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AthleteDto>> Handle(GetAthletesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Athletes
            .AsNoTracking()
            .Where(a => !a.IsDeleted);

        if (request.TenantId.HasValue)
        {
            query = query.Where(a => a.TenantId == request.TenantId.Value);
        }

        var athletes = await query.ToListAsync(cancellationToken);
        return athletes.Select(a => a.ToDto());
    }
}
