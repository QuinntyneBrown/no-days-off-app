using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Media;

namespace Media.Core.Features.GetMediaFiles;

public record GetMediaFilesQuery(int TenantId, int? EntityId = null, string? EntityType = null) : IRequest<IEnumerable<MediaFileDto>>;

public class GetMediaFilesHandler : IRequestHandler<GetMediaFilesQuery, IEnumerable<MediaFileDto>>
{
    private readonly IMediaDbContext _context;

    public GetMediaFilesHandler(IMediaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MediaFileDto>> Handle(GetMediaFilesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MediaFiles.Where(m => m.TenantId == request.TenantId);

        if (request.EntityId.HasValue)
            query = query.Where(m => m.EntityId == request.EntityId);

        if (!string.IsNullOrEmpty(request.EntityType))
            query = query.Where(m => m.EntityType == request.EntityType);

        return await query.Select(m => new MediaFileDto
        {
            MediaFileId = m.Id,
            FileName = m.FileName,
            OriginalFileName = m.OriginalFileName,
            ContentType = m.ContentType,
            Size = m.Size,
            Type = (int)m.Type,
            TenantId = m.TenantId,
            EntityId = m.EntityId,
            EntityType = m.EntityType,
            UploadedAt = m.UploadedAt,
            UploadedBy = m.UploadedBy
        }).ToListAsync(cancellationToken);
    }
}
