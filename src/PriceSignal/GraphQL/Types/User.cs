using Application.Common.Interfaces;
using Domain.Models.PriceRule;
using Domain.Models.User;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PriceSignal.GraphQL.Types;

public class UserType: ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Id).Type<NonNullType<IdType>>().Name("id");
        descriptor.Field(x => x.Email).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.NotificationChannels).Type<ListType<NonNullType<UserNotificationChannelType>>>()
            .UsePaging()
            .UseProjection()
            .Resolve(context =>
            {
                var user = context.Service<IUser>();
                var dbContext = context.Services.GetRequiredService<IAppDbContext>();
                return dbContext.UserNotificationChannels.Where(x => x.User.Id == user.UserIdentifier).AsQueryable();
            });
    }
}