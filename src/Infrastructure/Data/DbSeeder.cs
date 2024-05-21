using Domain.Models.Exchanges;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbSeeder
{
    public static void Initialize(AppDbContext context)
    {
        
        context.Database.Migrate();
        
        if (context.Exchanges.Any())
        {
            return;
        }
        
        context.Exchanges.AddRange(
            new Exchange {Name = "Binance", Description = "Binance Exchange"},
            new Exchange {Name = "Coinbase", Description = "Coinbase Exchange"},
            new Exchange {Name = "Kraken", Description = "Kraken Exchange"},
            new Exchange {Name = "DexScreener", Description = "DexScreener Exchange"}
        );
        
        context.SaveChanges();
    }
}