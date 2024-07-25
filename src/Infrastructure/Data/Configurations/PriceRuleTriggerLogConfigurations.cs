using Domain.Models.PriceRule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PriceRuleTriggerLogConfigurations : IEntityTypeConfiguration<PriceRuleTriggerLog>
{
    public void Configure(EntityTypeBuilder<PriceRuleTriggerLog> builder)
    {
        builder.Property(pc => pc.EntityId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();

        builder.Property(pr => pr.Price)
            .HasColumnType("double precision")
            .HasDefaultValue(null);
        
        builder.Property(pr => pr.PriceChange)
            .HasColumnType("double precision")
            .HasDefaultValue(null);
        
        builder.Property(pr => pr.PriceChangePercentage)
            .HasColumnType("double precision")
            .HasDefaultValue(null);

        builder.Property(pr => pr.PriceRuleSnapshot)
            .HasColumnType("jsonb");
        
        builder.Property(pr => pr.TriggeredAt)
            .IsRequired();
        
        builder.Property(pr=>pr.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();
        
        builder.Property(pr=>pr.ModifiedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnUpdate();

        builder.Property(pr => pr.DeletedAt)
            .HasDefaultValue(null);

        builder.HasOne(r => r.PriceRule)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasQueryFilter(pr => pr.DeletedAt == null);
    }
}