using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Athletes;

namespace Athletes.Core.Features.Athletes.RecordWeight;

public sealed class RecordWeightCommandHandler : IRequestHandler<RecordWeightCommand, AthleteDto>
{
    private readonly IAthletesDbContext _context;

    public RecordWeightCommandHandler(IAthletesDbContext context)
    {
        _context = context;
    }

    public async Task<AthleteDto> Handle(RecordWeightCommand request, CancellationToken cancellationToken)
    {
        var athlete = await _context.Athletes
            .FirstOrDefaultAsync(a => a.Id == request.AthleteId && !a.IsDeleted, cancellationToken)
            ?? throw new KeyNotFoundException($"Athlete {request.AthleteId} not found");

        athlete.RecordWeight(request.WeightInKgs, request.WeighedOn, request.RecordedBy);
        await _context.SaveChangesAsync(cancellationToken);

        return athlete.ToDto();
    }
}
