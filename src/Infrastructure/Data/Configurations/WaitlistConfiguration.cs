using Domain.Models.Waitlist;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class WaitlistConfiguration : BaseAuditableEntityConfiguration<Waitlist>
{
    public override void Configure(EntityTypeBuilder<Waitlist> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}