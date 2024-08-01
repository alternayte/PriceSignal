using Application.Common.Interfaces;
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
        Domain.Models.PriceRule.PriceRule priceRule = null;


        When()
            .Match<Domain.Models.PriceRule.PriceRule>(() => priceRule)
            .Exists<Domain.Models.PriceRule.PriceRule>(r => r.HasAttempted);

        Then()
            .Do(ctx => NotifyRuleTrigger(ctx,priceRule));
    }
    
    private void NotifyRuleTrigger(IContext ctx, Domain.Models.PriceRule.PriceRule rule)
    {
        try
        {
            _logger = ctx.Resolve<ILogger<PriceRuleNotificationRule>>();
            //_mediator = ctx.Resolve<IMediator>();
            _serviceProvider = ctx.Resolve<IServiceProvider>();
            using var scope = _serviceProvider.CreateScope();
            // _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            rule.HasAttempted = false;
            _context.PriceRules.Update(rule);
            _context.SaveChanges();
        } catch (Exception e)
        {
            // ignored
        }

        
        // foreach (var @event in rule.Events)
        // {
        //     _mediator.Publish(@event);
        // }
        // rule.ClearEvents();

        _logger.LogInformation("Price rule triggered: {Rule}", rule.Name);
    }
}