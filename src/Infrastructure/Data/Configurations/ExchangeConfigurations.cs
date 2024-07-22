using Domain.Models.Exchanges;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ExchangeConfigurations : IEntityTypeConfiguration<Exchange>
{
    public void Configure(EntityTypeBuilder<Exchange> builder)
    {
        builder.Property(e=> e.EntityId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();
        
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(pr=>pr.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();
        
        builder.Property(pr=>pr.ModifiedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnUpdate();

        builder.Property(pr => pr.DeletedAt)
            .HasDefaultValue(null);
        
        builder.HasQueryFilter(e => e.DeletedAt == null);
    }
}