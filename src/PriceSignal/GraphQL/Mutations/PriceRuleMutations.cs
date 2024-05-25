using Domain.Models.Instruments;
using Domain.Models.PriceRule;
using Infrastructure.Data;
using PriceSignal.BackgroundServices;

namespace PriceSignal.GraphQL.Mutations;

// [MutationType]
// public class PriceRuleMutations
// {
//     [UseMutationConvention]
//     public async Task<PriceRule> CreatePriceRule(string symbol,AppDbContext dbContext, [Service] IServiceProvider serviceProvider)
//     {
//
//             var binanceProcessingService = serviceProvider.GetRequiredService<BinancePriceFetcherService>();
//             await binanceProcessingService.UpdateSubscriptionsAsync();
//
//         var priceRule = new PriceRule
//         {
//             Description = "When the price of BTCUSDT is greater than 50000",
//             Instrument = new Instrument
//             {
//                 Symbol = "BTCUSDT",
//                 Name = "Bitcoin",
//                 BaseAsset = "BTC",
//                 QuoteAsset = "USDT"
//             },
//             Name = null
//         };
//         return priceRule;
//     }
// }