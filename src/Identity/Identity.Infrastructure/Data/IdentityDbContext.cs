using Identity.Core;
using Identity.Core.Aggregates.Tenant;
using Identity.Core.Aggregates.User;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data;

/// <summary>
/// Entity Framework Core DbContext for Identity service
/// </summary>
public class IdentityDbContext : DbContext, IIdentityDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Tenant> Tenants => Set<Tenant>();

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

        // Seed default roles
        modelBuilder.Entity<Role>().HasData(
            new { Id = 1, Name = Role.Names.Admin, Description = "Administrator role" },
            new { Id = 2, Name = Role.Names.User, Description = "Standard user role" },
            new { Id = 3, Name = Role.Names.Coach, Description = "Coach role" },
            new { Id = 4, Name = Role.Names.Athlete, Description = "Athlete role" }
        );
    }
}
