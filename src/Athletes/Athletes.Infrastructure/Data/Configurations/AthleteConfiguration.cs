using Athletes.Core.Aggregates.Athlete;
using Athletes.Core.Aggregates.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athletes.Infrastructure.Data.Configurations;

public class AthleteConfiguration : IEntityTypeConfiguration<Athlete>
{
    public void Configure(EntityTypeBuilder<Athlete> builder)
    {
        builder.ToTable("Athletes");

        builder.HasBaseType<Profile>();

        builder.Property(a => a.CurrentWeight);
        builder.Property(a => a.LastWeighedOn);

        builder.HasMany(a => a.Weights)
            .WithOne()
            .HasForeignKey("AthleteId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.CompletedExercises)
            .WithOne()
            .HasForeignKey("AthleteId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(a => a.Weights).AutoInclude(false);
        builder.Navigation(a => a.CompletedExercises).AutoInclude(false);
    }
}
