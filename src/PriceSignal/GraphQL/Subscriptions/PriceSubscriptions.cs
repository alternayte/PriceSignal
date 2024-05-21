using System.Runtime.CompilerServices;
using Domain.Models.Instruments;
using HotChocolate.Subscriptions;

namespace PriceSignal.GraphQL.Subscriptions;

[SubscriptionType]
public class PriceSubscriptions
{
    [Subscribe(With = nameof(SubscribeToUpdates))]
    public InstrumentPrice OnPriceUpdated(string symbol,[EventMessage] InstrumentPrice price)
    {
        return price;
    }
    
    public async IAsyncEnumerable<InstrumentPrice> SubscribeToUpdates(
        [Service] ITopicEventReceiver eventReceiver,
        string symbol,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {

        var stream = await eventReceiver.SubscribeAsync<InstrumentPrice>(
            nameof(OnPriceUpdated));
        await foreach (var price in stream.ReadEventsAsync().WithCancellation(cancellationToken))
        {
            if (price.Symbol == symbol)
            {
                yield return price;
            }
        }

    }
}