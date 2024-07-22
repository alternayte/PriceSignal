using Domain.Models.PriceRule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PriceConditionConfigurations : IEntityTypeConfiguration<PriceCondition>
{
    public void Configure(EntityTypeBuilder<PriceCondition> builder)
    {
        builder.Property(pc=> pc.EntityId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();
        
        builder.Property(pc=>pc.ConditionType)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(pc => pc.Value)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(pc => pc.AdditionalValues)
            .HasColumnType("jsonb");
        
        builder.Property(pr=>pr.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();
        
        builder.Property(pr=>pr.ModifiedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnUpdate();

        builder.Property(pr => pr.DeletedAt)
            .HasDefaultValue(null);
        
        builder.HasQueryFilter(pc => pc.DeletedAt == null);
    }
}