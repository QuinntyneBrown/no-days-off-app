using Microsoft.EntityFrameworkCore;
using Media.Core;
using Media.Core.Aggregates.MediaFile;

namespace Media.Infrastructure.Data;

public class MediaDbContext : DbContext, IMediaDbContext
{
    public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options)
    {
    }

    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MediaFile>(entity =>
        {
            entity.ToTable("MediaFiles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.OriginalFileName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.StoragePath).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.EntityType).HasMaxLength(100);
            entity.Property(e => e.UploadedBy).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => new { e.TenantId, e.EntityType, e.EntityId });
        });
    }
}
