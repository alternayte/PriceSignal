using Domain.Models.Exchanges;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ExchangeConfigurations : IEntityTypeConfiguration<Exchange>
{
    public void Configure(EntityTypeBuilder<Exchange> builder)
    {
        builder.Property(e=> e.EntityId)
            .HasDefaultValueSql("uuid_generate_v4()");
        
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.HasQueryFilter(e => e.DeletedAt == null);
    }
}