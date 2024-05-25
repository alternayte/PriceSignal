using Domain.Models.Instruments;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

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

public class TenMinCandleConfigurations : IEntityTypeConfiguration<TenMinCandle>
{
    public void Configure(EntityTypeBuilder<TenMinCandle> builder)
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
        builder.HasNoKey().ToView("ten_min_candle");
    }
}

public class FifteenMinCandleConfigurations : IEntityTypeConfiguration<FifteenMinCandle>
{
    public void Configure(EntityTypeBuilder<FifteenMinCandle> builder)
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
        builder.HasNoKey().ToView("fifteen_min_candle");
    }
}

public class OneHourCandleConfigurations : IEntityTypeConfiguration<OneHourCandle>
{
    public void Configure(EntityTypeBuilder<OneHourCandle> builder)
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
        builder.HasNoKey().ToView("one_hour_candle");
    }
}
