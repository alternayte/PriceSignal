using Skender.Stock.Indicators;

namespace Application.Price;

public interface IPrice : IQuote
{
    public string Symbol { get; set; }
    
    // public Exchange Exchange { get; init; } = default!;

    public void Update(decimal price, decimal quantity);
}
public class PriceQuote(Domain.Models.Instruments.Price price) : IPrice
{
    public DateTime Date => price.Bucket.DateTime;
    public decimal Open => price.Open;
    public decimal High => price.High;
    public decimal Low => price.Low;
    public decimal Close => price.Close;
    public decimal Volume => price.Volume;
    public string Symbol { get; set; } = price.Symbol;

    public void Update(decimal price, decimal quantity)
    {
        throw new NotImplementedException();
    }
}