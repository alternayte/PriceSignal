using Domain.Models.Instruments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class InstrumentConfigurations : IEntityTypeConfiguration<Instrument>
{
    public void Configure(EntityTypeBuilder<Instrument> builder)
    {
        builder.Property(i=> i.EntityId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();
        
        builder.Property(i => i.Symbol)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(i => i.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(i => i.Description)
            .HasMaxLength(2000);

        builder.Property(i => i.BaseAsset)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(i => i.QuoteAsset)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(pr=>pr.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();
        
        builder.Property(pr=>pr.ModifiedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnUpdate();

        builder.Property(pr => pr.DeletedAt)
            .HasDefaultValue(null);

        builder.HasQueryFilter(i => i.DeletedAt == null);
    }
}