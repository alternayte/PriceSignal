using Domain.Models.PriceRule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PriceRuleConfigurations : IEntityTypeConfiguration<PriceRule>
{
    public void Configure(EntityTypeBuilder<PriceRule> builder)
    {
        builder.Property(pc => pc.EntityId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();
        
        builder.Property(pr => pr.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(pr => pr.Description)
            .HasMaxLength(2000);
        
        builder.Property(pr=>pr.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();
        
        builder.Property(pr=>pr.ModifiedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnUpdate();

        builder.Property(pr => pr.DeletedAt)
            .HasDefaultValue(null);

        builder.HasMany(r => r.Conditions);
        builder.HasQueryFilter(pr => pr.DeletedAt == null);
    }
}