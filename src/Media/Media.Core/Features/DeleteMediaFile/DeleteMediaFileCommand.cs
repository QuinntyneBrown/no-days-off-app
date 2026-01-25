using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;
using Shared.Messaging;
using Shared.Messaging.Messages.Media;
using Media.Core.Aggregates.MediaFile;
using Media.Core.Services;

namespace Media.Core.Features.DeleteMediaFile;

public record DeleteMediaFileCommand(int MediaFileId, int TenantId) : IRequest<bool>;

public class DeleteMediaFileHandler : IRequestHandler<DeleteMediaFileCommand, bool>
{
    private readonly IMediaDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IMessageBus _messageBus;

    public DeleteMediaFileHandler(
        IMediaDbContext context,
        IFileStorageService fileStorage,
        IMessageBus messageBus)
    {
        _context = context;
        _fileStorage = fileStorage;
        _messageBus = messageBus;
    }

    public async Task<bool> Handle(DeleteMediaFileCommand request, CancellationToken cancellationToken)
    {
        var mediaFile = await _context.MediaFiles
            .Where(m => m.Id == request.MediaFileId && m.TenantId == request.TenantId)
            .FirstOrDefaultAsync(cancellationToken);

        if (mediaFile == null)
            throw new NotFoundException(nameof(MediaFile), request.MediaFileId);

        await _fileStorage.DeleteAsync(mediaFile.StoragePath, cancellationToken);

        _context.MediaFiles.Remove(mediaFile);
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(
            new MediaDeletedMessage(mediaFile.Id, mediaFile.TenantId),
            MessageTopics.Media.MediaDeleted,
            cancellationToken);

        return true;
    }
}
