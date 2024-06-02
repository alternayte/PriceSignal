using Application.TechnicalAnalysis.Rules;
using NRules.Extensibility;

namespace Application.Rules;

using NRules;
using NRules.Fluent;
using NRules.RuleModel;
using System;

public class RuleEngineConfig
{
    private readonly ISessionFactory _sessionFactory;

    public RuleEngineConfig(IServiceProvider serviceProvider)
    {
        var repository = new RuleRepository();
        repository.Load(x => x.From(typeof(TechnicalAnalysisRule).Assembly));

        _sessionFactory = repository.Compile();

        // Add dependencies to the session factory
        _sessionFactory.DependencyResolver = new DependencyResolver(serviceProvider);
                // SetDependencyResolver(new DependencyResolver(serviceProvider));
    }

    public ISession CreateSession()
    {
        return _sessionFactory.CreateSession();
    }
}

public class DependencyResolver : IDependencyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public DependencyResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object Resolve(IResolutionContext context, Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
}
