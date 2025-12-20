using MediatR;
using NoDaysOff.Core;
using NoDaysOff.Core.Model.VideoAggregate;

namespace NoDaysOff.Api;

public sealed class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, VideoDto>
{
    private readonly INoDaysOffContext _context;

    public CreateVideoCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<VideoDto> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
    {
        var video = Video.Create(
            request.TenantId,
            request.Title,
            request.YouTubeVideoId,
            request.CreatedBy);

        _context.Videos.Add(video);

        await _context.SaveChangesAsync(cancellationToken);

        return video.ToDto();
    }
}
