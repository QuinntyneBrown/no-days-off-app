using Identity.Core.Aggregates.Tenant;
using Identity.Core.Aggregates.User;
using Microsoft.EntityFrameworkCore;

namespace Identity.Core;

/// <summary>
/// Interface for Identity database context
/// </summary>
public interface IIdentityDbContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Tenant> Tenants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
