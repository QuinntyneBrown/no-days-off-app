using MediatR;
using Microsoft.EntityFrameworkCore;
using NoDaysOff.Core;

namespace NoDaysOff.Api;

public sealed class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, VideoDto>
{
    private readonly INoDaysOffContext _context;

    public UpdateVideoCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task<VideoDto> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
    {
        var video = await _context.Videos
            .FirstOrDefaultAsync(x => x.Id == request.VideoId, cancellationToken)
            ?? throw new InvalidOperationException($"Video with id {request.VideoId} not found.");

        video.UpdateTitle(request.Title, request.ModifiedBy);
        video.UpdateContent(request.SubTitle, request.Abstract, request.Description, request.ModifiedBy);

        if (request.Category is not null)
        {
            video.UpdateCategory(request.Category, request.ModifiedBy);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return video.ToDto();
    }
}
