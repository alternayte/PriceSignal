using Application.Common.Interfaces;
using Domain.Models.User;
using Infrastructure.Data;

namespace PriceSignal.GraphQL.Queries;

[QueryType]
public class UserQueries
{
    [UseSingleOrDefault]
    [UseProjection]
    public IQueryable<User> GetUser(AppDbContext dbContext, [Service] IUser user)
    {
        return dbContext.Users
            .Where(pr => pr.Id == user.UserIdentifier)
            .AsQueryable();
    }
}