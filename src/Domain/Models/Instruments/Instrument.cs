using Domain.Models.Exchanges;

namespace Domain.Models.Instruments;

public class Instrument : BaseAuditableEntity
{
    public required string Symbol { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string BaseAsset { get; init; } 
    public required string QuoteAsset { get; init; }
    public Exchange Exchange { get; init; } = default!;
    
}