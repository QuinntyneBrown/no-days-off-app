using Microsoft.EntityFrameworkCore;
using Communication.Core;
using Communication.Core.Aggregates.Notification;

namespace Communication.Infrastructure.Data;

public class CommunicationDbContext : DbContext, ICommunicationDbContext
{
    public CommunicationDbContext(DbContextOptions<CommunicationDbContext> options) : base(options)
    {
    }

    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notifications");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.ActionUrl).HasMaxLength(500);
            entity.Property(e => e.EntityType).HasMaxLength(100);
            entity.HasIndex(e => new { e.TenantId, e.UserId, e.IsRead });
            entity.HasIndex(e => e.CreatedAt);
        });
    }
}
