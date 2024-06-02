using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;

namespace Application.Services.Binance.Models;

public record KlineData(
    long OpenTime,
    decimal OpenPrice,
    decimal HighPrice,
    decimal LowPrice,
    decimal ClosePrice,
    decimal Volume,
    long CloseTime,
    decimal QuoteAssetVolume,
    long NumberOfTrades,
    decimal TakerBuyBaseAssetVolume,
    decimal TakerBuyQuoteAssetVolume
);

public record KlineParams
{
    [AliasAs("symbol")]
    public string Symbol { get; set; }
    [AliasAs("interval")]
    public KlineInterval Interval { get; set; }
    [AliasAs("startTime")]
    public long? StartTime { get; set; }
    [AliasAs("endTime")]
    public long? EndTime { get; set; }
}

public enum KlineInterval
{
    [EnumMember(Value = "1m")]
    OneMinute,
    [EnumMember(Value = "3m")]
    ThreeMinutes,
    [EnumMember(Value = "5m")]
    FiveMinutes,
    [EnumMember(Value = "15m")]
    FifteenMinutes,
    [EnumMember(Value = "30m")]
    ThirtyMinutes,
}

public static class KlineIntervalExtensions
{
    public static string ToApiString(this KlineInterval interval)
    {
        var enumType = typeof(KlineInterval);
        var name = Enum.GetName(enumType, interval);
        if (name != null)
        {
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }

        throw new ArgumentOutOfRangeException(nameof(interval), interval, null);
    }
}
// public static class KlineInterval
// {
//     public static readonly string OneMinute = "1m";
//     public static readonly string ThreeMinutes = "3m";
//     public static readonly string FiveMinutes = "5m";
//     public static readonly string FifteenMinutes = "15m";
//     public static readonly string ThirtyMinutes = "30m";
//     public static readonly string OneHour = "1h";
// }

