using Microsoft.EntityFrameworkCore;

namespace Media.Core;

public interface IMediaDbContext
{
    DbSet<Aggregates.MediaFile.MediaFile> MediaFiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
