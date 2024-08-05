using Domain.Models.NotificationChannel;
using Domain.Models.PriceRule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Configurations;

public class PriceRuleConfigurations : IEntityTypeConfiguration<PriceRule>
{
    public void Configure(EntityTypeBuilder<PriceRule> builder)
    {
        builder.Property(pc => pc.EntityId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();
        
        builder.Property(pr => pr.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(pr => pr.Description)
            .HasMaxLength(2000);
        
        builder.Property(pr => pr.IsEnabled)
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.Property(pr => pr.LastTriggeredAt)
            .HasDefaultValue(null);

        builder.Property(pr => pr.NotificationChannel)
            .HasDefaultValue(NotificationChannelType.none)
            .HasColumnType("notification_channel_type")
            .IsRequired();
            // .HasConversion(new EnumToStringConverter<NotificationChannelType>());
            // .HasConversion(v => v.ToString(),
            //     v => (NotificationChannelType) Enum.Parse(typeof(NotificationChannelType), v, true));
            
        builder.Property(pr=>pr.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();
        
        builder.Property(pr=>pr.ModifiedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnUpdate();

        builder.Property(pr => pr.DeletedAt)
            .HasDefaultValue(null);

        builder.Property(pr => pr.LastTriggeredPrice)
            .HasColumnType("double precision")
            .HasDefaultValue(null);

        builder.HasMany(r => r.Conditions);
        builder.HasMany(r => r.ActivationLogs);
        builder.HasQueryFilter(pr => pr.DeletedAt == null);
    }
}