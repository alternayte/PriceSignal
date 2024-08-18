using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasIndex(u => u.Id)
            .IsUnique();

        builder
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Id)
            .HasMaxLength(255);
        
        builder.Property(u => u.StripeCustomerId)
            .HasMaxLength(255);

        builder.HasMany(u => u.PriceRules)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Subscriptions)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Subscriptions)
            .WithOne(s => s.User);
            // .HasForeignKey(u => u.Id);

            builder.HasMany(r => r.PriceRules)
                .WithOne(pr => pr.User);
            // .HasPrincipalKey(u => u.Id);

            builder.HasMany(r => r.NotificationChannels)
                .WithOne(nc => nc.User);
    }
}