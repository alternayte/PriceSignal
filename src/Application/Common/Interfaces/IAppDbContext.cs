using Domain.Models.Exchanges;
using Domain.Models.Instruments;
using Domain.Models.PriceRule;
using Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    public DbSet<Exchange> Exchanges { get;  }
    public DbSet<Instrument> Instruments { get;  }
    public DbSet<InstrumentPrice> InstrumentPrices { get;  }
    public DbSet<PriceRuleTriggerLog> PriceRuleTriggerLogs { get;  }
    public DbSet<Domain.Models.PriceRule.PriceRule> PriceRules { get;  }
    public DbSet<User> Users { get;  }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    int SaveChanges();
}