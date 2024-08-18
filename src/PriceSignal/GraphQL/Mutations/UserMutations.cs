using System.Text.Json;
using Application.Common.Interfaces;
using Domain.Models.NotificationChannel;
using Domain.Models.PriceRule;
using Domain.Models.User;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PriceSignal.BackgroundServices;

namespace PriceSignal.GraphQL.Mutations;

[MutationType]
public class UserMutations
{
    public async Task<Guid> DeleteTelegramConnection(AppDbContext dbContext, [Service] IServiceProvider serviceProvider, [Service] IUser user)
    {
        var notification = await dbContext.UserNotificationChannels
            .FirstOrDefaultAsync(i => i.User.Id == user.UserIdentifier && i.ChannelType == NotificationChannelType.telegram);
        if (notification == null)
        {
            throw new InvalidOperationException("Notification not found");
        }

        dbContext.UserNotificationChannels.Remove(notification);
        await dbContext.SaveChangesAsync();
        return notification.EntityId;
    }
}