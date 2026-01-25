using Microsoft.EntityFrameworkCore;
using Exercises.Core;
using Exercises.Core.Aggregates.BodyPart;
using Exercises.Core.Aggregates.Exercise;

namespace Exercises.Infrastructure.Data;

public class ExercisesDbContext : DbContext, IExercisesDbContext
{
    public ExercisesDbContext(DbContextOptions<ExercisesDbContext> options) : base(options)
    {
    }

    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<BodyPart> BodyParts => Set<BodyPart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.ToTable("Exercises");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.VideoUrl).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Instructions).HasMaxLength(4000);
            entity.HasIndex(e => new { e.TenantId, e.Name });
        });

        modelBuilder.Entity<BodyPart>(entity =>
        {
            entity.ToTable("BodyParts");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.HasIndex(e => new { e.TenantId, e.Name }).IsUnique();
        });
    }
}
