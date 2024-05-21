using System.Reflection;
using Application.Common.Interfaces;
using Domain.Models.Exchanges;
using Domain.Models.Instruments;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Exchange> Exchanges => Set<Exchange>();
    public DbSet<Instrument> Instruments => Set<Instrument>();
    public DbSet<InstrumentPrice> InstrumentPrices => Set<InstrumentPrice>();
    public DbSet<OneMinCandle> OneMinCandle => Set<OneMinCandle>();
    public DbSet<FiveMinCandle> FiveMinCandle => Set<FiveMinCandle>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresExtension("uuid-ossp");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }
    
}