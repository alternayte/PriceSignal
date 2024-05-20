namespace Infrastructure.Providers.Binance.Models;


public record ExchangeInfo(
    string timezone,
    long serverTime,
    RateLimits[] rateLimits,
    object[] exchangeFilters,
    Symbols[] symbols
);

public record RateLimits(

);

public record Symbols(
    string symbol,
    string status,
    string baseAsset,
    int baseAssetPrecision,
    string quoteAsset,
    int quotePrecision,
    int quoteAssetPrecision,
    string[] orderTypes,
    bool icebergAllowed,
    bool ocoAllowed,
    bool quoteOrderQtyMarketAllowed,
    bool allowTrailingStop,
    bool cancelReplaceAllowed,
    bool isSpotTradingAllowed,
    bool isMarginTradingAllowed,
    object[] filters,
    string[] permissions,
    string defaultSelfTradePreventionMode,
    string[] allowedSelfTradePreventionModes
);

