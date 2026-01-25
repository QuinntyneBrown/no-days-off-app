using Athletes.Core;
using Athletes.Core.Aggregates.Athlete;
using Athletes.Core.Aggregates.Profile;
using Microsoft.EntityFrameworkCore;

namespace Athletes.Infrastructure.Data;

/// <summary>
/// Entity Framework Core DbContext for Athletes service
/// </summary>
public class AthletesDbContext : DbContext, IAthletesDbContext
{
    public DbSet<Athlete> Athletes => Set<Athlete>();
    public DbSet<Profile> Profiles => Set<Profile>();

    public AthletesDbContext(DbContextOptions<AthletesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AthletesDbContext).Assembly);
    }
}
