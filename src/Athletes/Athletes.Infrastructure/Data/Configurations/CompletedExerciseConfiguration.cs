using Athletes.Core.Aggregates.Athlete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athletes.Infrastructure.Data.Configurations;

public class CompletedExerciseConfiguration : IEntityTypeConfiguration<CompletedExercise>
{
    public void Configure(EntityTypeBuilder<CompletedExercise> builder)
    {
        builder.ToTable("CompletedExercises");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.ScheduledExerciseId)
            .IsRequired();

        builder.Property(c => c.CompletionDateTime)
            .IsRequired();

        builder.Property(c => c.RecordedBy)
            .IsRequired()
            .HasMaxLength(256);
    }
}
