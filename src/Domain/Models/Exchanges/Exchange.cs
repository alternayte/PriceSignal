using Domain.Models.Instruments;

namespace Domain.Models.Exchanges;

public class Exchange : BaseAuditableEntity
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public IList<Instrument> Instruments { get; init; } = new List<Instrument>();
}