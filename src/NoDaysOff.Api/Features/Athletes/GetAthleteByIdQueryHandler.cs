using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetAthleteByIdQueryHandler : IRequestHandler<GetAthleteByIdQuery, AthleteDto?>
{
    private readonly INoDaysOffContext _context;

    public GetAthleteByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<AthleteDto?> Handle(GetAthleteByIdQuery request, CancellationToken cancellationToken)
    {
        var athlete = await _context.Athletes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.AthleteId && !x.IsDeleted, cancellationToken);

        return athlete?.ToDto();
    }
}
