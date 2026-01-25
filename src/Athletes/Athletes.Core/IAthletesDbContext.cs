using Athletes.Core.Aggregates.Athlete;
using Athletes.Core.Aggregates.Profile;
using Microsoft.EntityFrameworkCore;

namespace Athletes.Core;

/// <summary>
/// Interface for Athletes database context
/// </summary>
public interface IAthletesDbContext
{
    DbSet<Athlete> Athletes { get; }
    DbSet<Profile> Profiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
