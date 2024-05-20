using Domain.Models.Instruments;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PriceConfigurations : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.Property(p => p.Open)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.High)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.Low)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.Close)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.Volume)
            .HasColumnType("double precision");
        
        builder.Property(p => p.Bucket)
            .HasDefaultValueSql("now()")
            .IsRequired();
        
        //builder.HasNoKey().ToView("one_min_candle");
        builder.HasNoKey();

    }
    
    // public void Configure(EntityTypeBuilder<OneMinCandle> builder)
    // {
    //     builder.HasBaseType<Price>();
    //     builder.HasNoKey().ToView("one_min_candle");
    // }
    //
    // public void Configure(EntityTypeBuilder<FiveMinCandle> builder)
    // {
    //     builder.HasBaseType<Price>();
    //     builder.HasNoKey().ToView("five_min_candle");
    // }
}

public class OneMinCandleConfigurations : IEntityTypeConfiguration<OneMinCandle>
{
    public void Configure(EntityTypeBuilder<OneMinCandle> builder)
    {
        builder.Property(p => p.Open)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.High)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.Low)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.Close)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.Volume)
            .HasColumnType("double precision");
        
        builder.Property(p => p.Bucket)
            .HasDefaultValueSql("now()")
            .IsRequired();
        
        builder.HasNoKey().ToView("one_min_candle");
    }
}

public class FiveMinCandleConfigurations : IEntityTypeConfiguration<FiveMinCandle>
{
    public void Configure(EntityTypeBuilder<FiveMinCandle> builder)
    {
        builder.Property(p => p.Open)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.High)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.Low)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.Close)
            .HasColumnType("double precision")
            .IsRequired();
        
        builder.Property(p => p.Volume)
            .HasColumnType("double precision");
        
        builder.Property(p => p.Bucket)
            .HasDefaultValueSql("now()")
            .IsRequired();
        builder.HasNoKey().ToView("five_min_candle");
    }
}
