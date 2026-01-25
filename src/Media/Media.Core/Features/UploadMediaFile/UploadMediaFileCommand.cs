using MediatR;
using Shared.Contracts.Media;
using Shared.Messaging;
using Shared.Messaging.Messages.Media;
using Media.Core.Aggregates.MediaFile;
using Media.Core.Services;

namespace Media.Core.Features.UploadMediaFile;

public record UploadMediaFileCommand(
    Stream FileStream,
    string OriginalFileName,
    string ContentType,
    long Size,
    int TenantId,
    string UploadedBy,
    int? EntityId = null,
    string? EntityType = null) : IRequest<MediaFileDto>;

public class UploadMediaFileHandler : IRequestHandler<UploadMediaFileCommand, MediaFileDto>
{
    private readonly IMediaDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IMessageBus _messageBus;

    public UploadMediaFileHandler(
        IMediaDbContext context,
        IFileStorageService fileStorage,
        IMessageBus messageBus)
    {
        _context = context;
        _fileStorage = fileStorage;
        _messageBus = messageBus;
    }

    public async Task<MediaFileDto> Handle(UploadMediaFileCommand request, CancellationToken cancellationToken)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.OriginalFileName)}";
        var storagePath = await _fileStorage.UploadAsync(
            request.FileStream, fileName, request.ContentType, cancellationToken);

        var mediaType = DetermineMediaType(request.ContentType);

        var mediaFile = new MediaFile(
            fileName,
            request.OriginalFileName,
            request.ContentType,
            request.Size,
            storagePath,
            mediaType,
            request.TenantId,
            request.UploadedBy,
            request.EntityId,
            request.EntityType);

        _context.MediaFiles.Add(mediaFile);
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(
            MessageTopics.MediaUploaded,
            new MediaUploadedMessage(mediaFile.Id, mediaFile.FileName, mediaFile.TenantId),
            cancellationToken);

        return new MediaFileDto
        {
            MediaFileId = mediaFile.Id,
            FileName = mediaFile.FileName,
            OriginalFileName = mediaFile.OriginalFileName,
            ContentType = mediaFile.ContentType,
            Size = mediaFile.Size,
            Type = (int)mediaFile.Type,
            TenantId = mediaFile.TenantId,
            EntityId = mediaFile.EntityId,
            EntityType = mediaFile.EntityType,
            UploadedAt = mediaFile.UploadedAt,
            UploadedBy = mediaFile.UploadedBy,
            Url = _fileStorage.GetPublicUrl(storagePath)
        };
    }

    private static MediaType DetermineMediaType(string contentType)
    {
        if (contentType.StartsWith("image/"))
            return MediaType.Image;
        if (contentType.StartsWith("video/"))
            return MediaType.Video;
        if (contentType.StartsWith("application/pdf") ||
            contentType.StartsWith("application/msword") ||
            contentType.StartsWith("application/vnd."))
            return MediaType.Document;
        return MediaType.Other;
    }
}
