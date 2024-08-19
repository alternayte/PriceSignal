using System.Collections.Concurrent;

namespace Application.Rules.Common;

public class RuleCache
{
    private readonly ConcurrentDictionary<long, Domain.Models.PriceRule.PriceRule> _rules = new();

    public IEnumerable<Domain.Models.PriceRule.PriceRule> GetAllRules()
    {
        return _rules.Values;
    }

    public Domain.Models.PriceRule.PriceRule GetRule(long ruleId)
    {
        _rules.TryGetValue(ruleId, out var rule);
        return rule;
    }

    public void AddOrUpdateRule(Domain.Models.PriceRule.PriceRule rule)
    {
        _rules.AddOrUpdate(rule.Id, rule, (key, oldValue) => rule);
    }

    public void RemoveRule(long ruleId)
    {
        _rules.TryRemove(ruleId, out _);
    }

    public void LoadRules(IEnumerable<Domain.Models.PriceRule.PriceRule> rules)
    {
        _rules.Clear();
        foreach (var rule in rules)
        {
            _rules[rule.Id] = rule;
        }
    }
}