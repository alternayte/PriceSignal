using Domain.Models.Instruments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class InstrumentPriceConfigurations : IEntityTypeConfiguration<InstrumentPrice>
{
    public void Configure(EntityTypeBuilder<InstrumentPrice> builder)
    {
        builder.Property(ip => ip.Price)
            .HasColumnType("double precision")
            .IsRequired();

        builder.Property(ip => ip.Volume)
            .HasColumnType("double precision");


        builder.Property(ip => ip.Quantity)
            .HasColumnType("double precision");


        builder.Property(ip => ip.Timestamp)
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasNoKey();
    }
}