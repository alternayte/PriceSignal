using Domain.Models.Exchanges;

namespace Domain.Models.Instruments;

public class InstrumentPrice : EventEntity
{
    public required string Symbol { get; init; }
    public  required decimal Price { get; init; }
    public decimal Volume { get; init; }
    public decimal Quantity { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public Exchange Exchange { get; init; } = default!;
}