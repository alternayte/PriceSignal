using Domain.Models.NotificationChannel;

namespace PriceSignal.GraphQL.Types;

public class NotificationChannelTypeType : EnumType<NotificationChannelType>
{
    protected override void Configure(IEnumTypeDescriptor<NotificationChannelType> descriptor)
    {
        descriptor.BindValuesExplicitly();
        descriptor.Value(NotificationChannelType.none).Name("NONE");
        descriptor.Value(NotificationChannelType.sms).Name("SMS");
        descriptor.Value(NotificationChannelType.email).Name("EMAIL");
        descriptor.Value(NotificationChannelType.webhook).Name("WEBHOOK");
        descriptor.Value(NotificationChannelType.telegram).Name("TELEGRAM");
        descriptor.Value(NotificationChannelType.push_notification).Name("PUSH_NOTIFICATION");
    }
}