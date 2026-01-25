using Identity.Core.Aggregates.Tenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.UniqueId)
            .IsRequired();

        builder.HasIndex(t => t.UniqueId)
            .IsUnique();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(Tenant.MaxNameLength);

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(256);

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
