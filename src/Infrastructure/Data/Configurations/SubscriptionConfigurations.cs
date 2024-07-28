using Domain.Models.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class SubscriptionConfigurations : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder
            .HasIndex(u => u.Id)
            .IsUnique();

        builder.Property(u => u.Id)
            .HasMaxLength(255);

        builder.Property(u => u.Status)
            .HasMaxLength(30);
        
        builder.Property(u => u.PriceId)
            .HasMaxLength(255);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("now()");

    }
}