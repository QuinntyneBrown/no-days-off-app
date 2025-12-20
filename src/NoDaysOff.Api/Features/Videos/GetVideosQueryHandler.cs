using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetVideosQueryHandler : IRequestHandler<GetVideosQuery, IEnumerable<VideoDto>>
{
    private readonly INoDaysOffContext _context;

    public GetVideosQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VideoDto>> Handle(GetVideosQuery request, CancellationToken cancellationToken)
    {
        return await _context.Videos
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);
    }
}
