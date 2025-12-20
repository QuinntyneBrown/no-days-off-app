using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class GetVideoByIdQueryHandler : IRequestHandler<GetVideoByIdQuery, VideoDto?>
{
    private readonly INoDaysOffContext _context;

    public GetVideoByIdQueryHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<VideoDto?> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
    {
        var video = await _context.Videos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.VideoId && !x.IsDeleted, cancellationToken);

        return video?.ToDto();
    }
}
