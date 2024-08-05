using System.Reflection;
using Application.Common.Interfaces;
using Domain.Models.Exchanges;
using Domain.Models.Instruments;
using Domain.Models.NotificationChannel;
using Domain.Models.PriceRule;
using Domain.Models.User;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserNotificationChannel> UserNotificationChannels => Set<UserNotificationChannel>();
    public DbSet<Exchange> Exchanges => Set<Exchange>();
    public DbSet<Instrument> Instruments => Set<Instrument>();
    public DbSet<PriceRule> PriceRules => Set<PriceRule>();
    public DbSet<PriceRuleTriggerLog> PriceRuleTriggerLogs => Set<PriceRuleTriggerLog>();
    public DbSet<PriceCondition> PriceConditions => Set<PriceCondition>();
    public DbSet<InstrumentPrice> InstrumentPrices => Set<InstrumentPrice>();
    public DbSet<OneMinCandle> OneMinCandle => Set<OneMinCandle>();
    public DbSet<FiveMinCandle> FiveMinCandle => Set<FiveMinCandle>();
    public DbSet<TenMinCandle> TenMinCandle => Set<TenMinCandle>();
    public DbSet<FifteenMinCandle> FifteenMinCandle => Set<FifteenMinCandle>();
    public DbSet<OneHourCandle> OneHourCandle => Set<OneHourCandle>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresExtension("uuid-ossp");
        builder.HasPostgresEnum<NotificationChannelType>(
            schema:null, name:"notification_channel_type");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }
    
}