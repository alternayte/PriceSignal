using System.Runtime.CompilerServices;
using Domain.Models.Instruments;
using HotChocolate.Subscriptions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PriceSignal.GraphQL.Subscriptions;

[SubscriptionType]
public class PriceSubscriptions
{
    
    [Subscribe(With = nameof(SubscribeToUpdates))]
    public Price? OnPriceUpdated(string symbol,[EventMessage] Price price)
    {
        return price;
    }
    
    public async IAsyncEnumerable<Price> SubscribeToUpdates(
        [Service] ITopicEventReceiver eventReceiver,
        string symbol,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {

        var stream = await eventReceiver.SubscribeAsync<Price>(
            nameof(OnPriceUpdated), cancellationToken);
        await foreach (var price in stream.ReadEventsAsync().WithCancellation(cancellationToken))
        {
            if (price.Symbol != symbol) continue;
            Console.WriteLine($"Price: c:{price.Close} h: {price.High} l: {price.Low} o: {price.Open}  v: {price.Volume}");
            // var priceItem = await _dbContext.OneMinCandle
            //     .Where(c => c.Symbol == symbol)
            //     .OrderByDescending(c => c.Bucket)
            //     .FirstOrDefaultAsync(cancellationToken);
            yield return price;
        }

    }
}