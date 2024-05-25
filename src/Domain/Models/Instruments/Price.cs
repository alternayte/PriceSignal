using Domain.Models.Exchanges;

namespace Domain.Models.Instruments;

public class Price
{
    public required string Symbol { get; init; }
    public  required decimal Open { get; set; }
    public  required decimal High { get; set; }
    public  required decimal Low { get; set; }
    public  required decimal Close { get; set; }
    public decimal Volume { get; set; }
    public DateTimeOffset Bucket { get; init; }
    public Exchange Exchange { get; init; } = default!;
    
    public void Update(decimal price, decimal quantity)
    {
        if (price > High) High = price;
        if (price < Low) Low = price;
        Close = price;
        Volume += quantity;
    }
}