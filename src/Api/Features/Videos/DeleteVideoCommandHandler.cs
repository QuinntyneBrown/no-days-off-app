using MediatR;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Api;

public sealed class DeleteVideoCommandHandler : IRequestHandler<DeleteVideoCommand>
{
    private readonly INoDaysOffContext _context;

    public DeleteVideoCommandHandler(INoDaysOffContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
    {
        var video = await _context.Videos
            .FirstOrDefaultAsync(x => x.Id == request.VideoId, cancellationToken)
            ?? throw new InvalidOperationException($"Video with id {request.VideoId} not found.");

        video.Delete();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
