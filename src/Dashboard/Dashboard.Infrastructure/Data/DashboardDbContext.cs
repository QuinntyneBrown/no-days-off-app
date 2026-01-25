using Microsoft.EntityFrameworkCore;
using Dashboard.Core;
using Dashboard.Core.Aggregates.Widget;
using Dashboard.Core.Aggregates.DashboardStats;

namespace Dashboard.Infrastructure.Data;

public class DashboardDbContext : DbContext, IDashboardDbContext
{
    public DashboardDbContext(DbContextOptions<DashboardDbContext> options) : base(options)
    {
    }

    public DbSet<Widget> Widgets => Set<Widget>();
    public DbSet<DashboardStats> DashboardStats => Set<DashboardStats>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Widget>(entity =>
        {
            entity.ToTable("Widgets");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Configuration).HasMaxLength(4000);
            entity.HasIndex(e => new { e.TenantId, e.UserId });
        });

        modelBuilder.Entity<DashboardStats>(entity =>
        {
            entity.ToTable("DashboardStats");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.UserId }).IsUnique();
        });
    }
}
