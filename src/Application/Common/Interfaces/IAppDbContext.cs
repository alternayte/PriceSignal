using Domain.Models.Exchanges;
using Domain.Models.Instruments;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    public DbSet<Exchange> Exchanges { get;  }
    public DbSet<Instrument> Instruments { get;  }
    public DbSet<InstrumentPrice> InstrumentPrices { get;  }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}