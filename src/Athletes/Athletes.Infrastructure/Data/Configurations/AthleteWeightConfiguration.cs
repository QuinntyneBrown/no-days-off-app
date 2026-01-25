using Athletes.Core.Aggregates.Athlete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athletes.Infrastructure.Data.Configurations;

public class AthleteWeightConfiguration : IEntityTypeConfiguration<AthleteWeight>
{
    public void Configure(EntityTypeBuilder<AthleteWeight> builder)
    {
        builder.ToTable("AthleteWeights");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.WeightInKgs)
            .IsRequired();

        builder.Property(w => w.WeighedOn)
            .IsRequired();

        builder.Property(w => w.RecordedBy)
            .IsRequired()
            .HasMaxLength(256);
    }
}
