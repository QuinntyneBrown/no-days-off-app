using Microsoft.EntityFrameworkCore;

namespace Exercises.Core;

public interface IExercisesDbContext
{
    DbSet<Aggregates.Exercise.Exercise> Exercises { get; }
    DbSet<Aggregates.BodyPart.BodyPart> BodyParts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
