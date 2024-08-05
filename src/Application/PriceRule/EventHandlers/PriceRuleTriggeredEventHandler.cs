using System.Text;
using Application.Common;
using Application.Common.Interfaces;
using Domain.Models.PriceRule;
using Domain.Models.PriceRule.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.PriceRule.EventHandlers;

public class PriceRuleTriggeredEventHandler : INotificationHandler<PriceRuleTriggeredEvent>
{
    private readonly ILogger<PriceRuleTriggeredEventHandler> _logger;
    private readonly IAppDbContext _context;
    private readonly NotificationService _notificationService;
    private TimeProvider _timeProvider;

    public PriceRuleTriggeredEventHandler(ILogger<PriceRuleTriggeredEventHandler> logger, IAppDbContext context, NotificationService notificationService,TimeProvider timeProvider)
    {
        _logger = logger;
        _context = context;
        _notificationService = notificationService;
        _timeProvider = timeProvider;
    }

    public async Task Handle(PriceRuleTriggeredEvent notification, CancellationToken cancellationToken)
    {
        // var nowTime = _timeProvider.GetUtcNow();
        var rule = notification.Rule;
        // do not trigger send if last trigger time has not passed 5 minute cooldown
        // if (rule.LastTriggeredAt.HasValue && rule.LastTriggeredAt.Value.AddMinutes(1) > nowTime)
        // {
        //     // _logger.LogInformation("Price rule domain event: {Event} | {Id} | Skipped", notification.GetType().Name, notification.Rule.EntityId);
        //     return;
        // }
        await _notificationService.SendAsync(rule.UserId, FormatRuleConditionsMessage(rule),
            rule.NotificationChannel);
        _logger.LogInformation("Price rule domain event: {Event} | {Id}", notification.GetType().Name, notification.Rule.EntityId);
        //rule.ActivationLogs.Add(new PriceRuleTriggerLog(rule));
        //_context.PriceRules.Update(rule);
        //_context.PriceRuleTriggerLogs.Add(new PriceRuleTriggerLog(rule));
        //await _context.SaveChangesAsync(cancellationToken);
    }

    private static string FormatRuleConditionsMessage(Domain.Models.PriceRule.PriceRule rule)
    {
        var message = new StringBuilder();
        message.AppendLine($"Rule: {rule.Name}");
        message.AppendLine($"Price: {rule.LastTriggeredPrice}");
        message.AppendLine($"Instrument: {rule.Instrument.Symbol}");
        message.AppendLine($"Conditions:");
        foreach (var condition in rule.Conditions)
        {
            var meta = condition.AdditionalValues.RootElement;
            message.AppendLine($"""
                                - {condition.ConditionType} has {meta.GetProperty("name").GetString()}({meta.GetProperty("period").GetInt32()}) going {meta.GetProperty("direction").GetString()} {condition.Value}
                                """);
        }
        return message.ToString();
    }
}