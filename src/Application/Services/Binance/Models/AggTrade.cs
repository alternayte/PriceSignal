using System.Text.Json.Serialization;
using Refit;

namespace Application.Services.Binance.Models;

public record AggTrade(
    long a,
    string p,
    string q,
    long f,
    long l,
    long T
    // [property: JsonPropertyName("m")]
    // bool m,
    // [property: JsonPropertyName("M")]
    // bool M
);

public class AggTradeParams
{
    [AliasAs("symbol")]
    public required string Symbol { get; set; }
    [AliasAs("startTime")]
    public long? StartTime { get; set; }
    [AliasAs("endTime")]
    public long? EndTime { get; set; }
}