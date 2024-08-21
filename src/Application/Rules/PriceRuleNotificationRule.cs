using Application.Common.Interfaces;
using Application.Price;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NRules.Fluent.Dsl;
using NRules.RuleModel;

namespace Application.Rules;

public class PriceRuleNotificationRule : Rule
{
    private ILogger<PriceRuleNotificationRule> _logger;
    private IMediator _mediator;
    private IServiceProvider _serviceProvider;
    private IAppDbContext _context;

    public PriceRuleNotificationRule(ILogger<PriceRuleNotificationRule> logger, IMediator mediator, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _mediator = mediator;
        _serviceProvider = serviceProvider;
    }
    
    public PriceRuleNotificationRule()
    {
    }

    public override void Define()
    {
        IPrice price = null;
        Domain.Models.PriceRule.PriceRule priceRule = null;
        
        When()
            .Match<IPrice>(() => price)
            .Match<Domain.Models.PriceRule.PriceRule>(() => priceRule)
            .Exists<Domain.Models.PriceRule.PriceRule>(r => r.HasAttempted);

        Then()
            .Do(ctx => NotifyRuleTrigger(ctx,price,priceRule));
    }
    
    private void NotifyRuleTrigger(IContext ctx,IPrice price, Domain.Models.PriceRule.PriceRule rule)
    {
        try
        {
            _logger = ctx.Resolve<ILogger<PriceRuleNotificationRule>>();
            _serviceProvider = ctx.Resolve<IServiceProvider>();
            using var scope = _serviceProvider.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            rule.HasAttempted = false;
            
            var nowTime = DateTime.UtcNow;
            // do not trigger send if last trigger time has not passed 5 minute cooldown
            // if (rule.LastTriggeredAt.HasValue && rule.LastTriggeredAt.Value.AddMinutes(5) > nowTime)
            // {
            //     // _logger.LogInformation("Price rule domain event: {Event} | {Id} | Skipped", notification.GetType().Name, notification.Rule.EntityId);
            //     return;
            // }
            rule.Trigger(price.Close);
            _context.PriceRules.Update(rule);
            _context.SaveChangesAsync().GetAwaiter().GetResult();
        } catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
        
        //_logger.LogInformation("Price rule triggered: {Rule}", rule.Name);
    }
}