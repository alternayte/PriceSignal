using Application.Common;
using Application.Common.Interfaces;
using Domain.Models.PriceRule;
using Domain.Models.PriceRule.Events;
using Microsoft.Extensions.Logging;

namespace Application.PriceRule.EventHandlers;

public class PriceRuleTriggeredEventHandler
{
    private readonly ILogger<PriceRuleTriggeredEventHandler> _logger;
    private readonly IAppDbContext _context;
    private readonly NotificationService _notificationService;

    public PriceRuleTriggeredEventHandler(ILogger<PriceRuleTriggeredEventHandler> logger, IAppDbContext context, NotificationService notificationService)
    {
        _logger = logger;
        _context = context;
        _notificationService = notificationService;
    }

    public async Task Handle(PriceRuleTriggeredEvent notification, CancellationToken cancellationToken)
    {
        var rule = notification.Rule;
        await _notificationService.SendAsync("1234", $"Rule triggered: {rule.Name} for {rule.Instrument.Symbol}.",
            rule.NotificationChannel);
        _logger.LogInformation("Price rule domain event: {Event} | {Id}", notification.GetType().Name, notification.Rule.EntityId);
        _context.PriceRules.Update(rule);
        _context.PriceRuleTriggerLogs.Add(new PriceRuleTriggerLog(rule));
        await _context.SaveChangesAsync(cancellationToken);
    }
}