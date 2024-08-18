using Domain.Models.NotificationChannel;
using Domain.Models.User;

namespace PriceSignal.GraphQL.Types;

public class UserNotificationChannelType : ObjectType<UserNotificationChannel>
{
    protected override void Configure(IObjectTypeDescriptor<UserNotificationChannel> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.EntityId).Type<NonNullType<IdType>>().Name("id");
        descriptor.Field(x => x.ChannelType).Type<NonNullType<NotificationChannelTypeType>>();
        descriptor.Field(x => x.TelegramChatId).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.TelegramUsername).Type<NonNullType<StringType>>(); }
}
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