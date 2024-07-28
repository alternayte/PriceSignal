using Application.Rules;
using Application.TechnicalAnalysis.Rules;
using Microsoft.Extensions.DependencyInjection;
using NRules;
using NRules.Fluent;
using NRules.RuleModel;
using NRules.Testing;

namespace Application.UnitTests.Common;

public class BaseRulesTestFixture : RulesTestFixture
{
    // protected ISession Session { get; private set; }
    protected IServiceProvider ServiceProvider { get; private set; }
    // protected RuleRepository Repository { get; private set; }
    // protected ISessionFactory Factory { get; private set; }
    // protected IRulesTestSetup Setup { get; private set; }


    protected BaseRulesTestFixture()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.BuildServiceProvider();
        ServiceProvider = services.BuildServiceProvider();

        // Repository = new RuleRepository();
        // Repository.Load(x => x.From(typeof(PriceRuleNotificationRule).Assembly));
        // // var activator = new CustomRuleActivator(ServiceProvider);
        // Factory = Repository.Compile();
        // Factory.DependencyResolver = new DependencyResolver(ServiceProvider);
        // Session = Factory.CreateSession();
        // Setup = new TestSetup(Session);
        Asserter = new XUnitRuleAsserter();
    }
}
//
// public class CustomRuleActivator : IRuleActivator
// {
//     private readonly IServiceProvider _serviceProvider;
//
//     public CustomRuleActivator(IServiceProvider serviceProvider)
//     {
//         _serviceProvider = serviceProvider;
//     }
//
//     public object Activate(Type ruleType)
//     {
//         return _serviceProvider.GetRequiredService(ruleType);
//     }
// }