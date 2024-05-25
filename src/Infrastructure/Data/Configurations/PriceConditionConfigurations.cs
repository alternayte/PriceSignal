using Domain.Models.PriceRule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PriceConditionConfigurations : IEntityTypeConfiguration<PriceCondition>
{
    public void Configure(EntityTypeBuilder<PriceCondition> builder)
    {
        builder.Property(pc=> pc.EntityId)
            .HasDefaultValueSql("uuid_generate_v4()");
        
        builder.Property(pc=>pc.ConditionType)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(pc => pc.Value)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(pc => pc.AdditionalValue)
            .HasColumnType("jsonb");
        
        builder.HasQueryFilter(pc => pc.DeletedAt == null);
    }
}