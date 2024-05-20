using Domain.Models.Exchanges;

namespace Domain.Models.Instruments;

public class Price
{
    public required string Symbol { get; init; }
    public  required decimal Open { get; init; }
    public  required decimal High { get; init; }
    public  required decimal Low { get; init; }
    public  required decimal Close { get; init; }
    public decimal Volume { get; init; }
    public DateTimeOffset Bucket { get; init; }
    public Exchange Exchange { get; init; } = default!;
}