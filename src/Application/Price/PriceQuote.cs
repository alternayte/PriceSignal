using Skender.Stock.Indicators;

namespace Application.Price;

public interface IPrice : IQuote
{
    public string Symbol { get; set; }
    
    // public Exchange Exchange { get; init; } = default!;

    public void Update(decimal uPrice, decimal quantity);
}
public class PriceQuote(Domain.Models.Instruments.Price price) : IPrice
{
    public DateTime Date => price.Bucket.DateTime;
    public decimal Open => price.Open;
    public decimal High { get; private set; } = price.High;
    public decimal Low { get; private set; } = price.Low;
    public decimal Close { get; private set; } = price.Close;
    public decimal Volume { get; private set; } = price.Volume;
    public string Symbol { get; set; } = price.Symbol;

    public void Update(decimal uPrice, decimal quantity)
    {
        if (uPrice > High) High = uPrice;
        if (uPrice < Low) Low = uPrice;
        Close = uPrice;
        Volume += quantity;
    }
}