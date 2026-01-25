using Athletes.Core.Aggregates.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Athletes.Infrastructure.Data.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Profile.MaxNameLength);

        builder.Property(p => p.Username)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(p => p.Username)
            .IsUnique();

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(2000);

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(256);

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.UseTptMappingStrategy();
    }
}
