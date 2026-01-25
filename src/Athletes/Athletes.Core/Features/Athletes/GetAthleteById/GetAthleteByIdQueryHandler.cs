using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.GetAthleteById;

public sealed class GetAthleteByIdQueryHandler : IRequestHandler<GetAthleteByIdQuery, AthleteDto?>
{
    private readonly IAthletesDbContext _context;

    public GetAthleteByIdQueryHandler(IAthletesDbContext context)
    {
        _context = context;
    }

    public async Task<AthleteDto?> Handle(GetAthleteByIdQuery request, CancellationToken cancellationToken)
    {
        var athlete = await _context.Athletes
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.AthleteId && !a.IsDeleted, cancellationToken);

        return athlete?.ToDto();
    }
}
