using Domain.Models.NotificationChannel;
using Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Configurations;

public class UserNotificationChannelConfiguration : IEntityTypeConfiguration<UserNotificationChannel>
{
    public void Configure(EntityTypeBuilder<UserNotificationChannel> builder)
    {
        builder.Property(unc => unc.EntityId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();
        
        builder.Property(unc=>unc.TelegramChatId)
            .HasDefaultValue(null);
        
        builder.Property(unc=>unc.TelegramUsername)
            .HasMaxLength(255)
            .HasDefaultValue(null);

        builder.Property(unc => unc.ChannelType)
            .HasColumnType("notification_channel_type")
            .IsRequired();
            // .HasConversion(new EnumToStringConverter<NotificationChannelType>());
            //
            // .HasConversion(v => v.ToString(),
            // v => (NotificationChannelType) Enum.Parse(typeof(NotificationChannelType), v, true));
        
        builder.Property(unc=>unc.CreatedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnAdd();
        
        builder.Property(unc=>unc.ModifiedAt)
            .HasDefaultValueSql("now()")
            .ValueGeneratedOnUpdate();

        builder.Property(unc => unc.DeletedAt)
            .HasDefaultValue(null);
        
        builder.HasQueryFilter(unc => unc.DeletedAt == null);

    }
}