using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.GetWeightHistory;

public sealed class GetWeightHistoryQueryHandler : IRequestHandler<GetWeightHistoryQuery, IEnumerable<AthleteWeightDto>>
{
    private readonly IAthletesDbContext _context;

    public GetWeightHistoryQueryHandler(IAthletesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AthleteWeightDto>> Handle(GetWeightHistoryQuery request, CancellationToken cancellationToken)
    {
        var athlete = await _context.Athletes
            .Include(a => a.Weights)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.AthleteId && !a.IsDeleted, cancellationToken)
            ?? throw new KeyNotFoundException($"Athlete {request.AthleteId} not found");

        return athlete.GetWeightHistory(request.Count).Select(w => w.ToDto());
    }
}
