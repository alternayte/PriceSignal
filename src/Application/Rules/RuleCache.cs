using System.Collections.Concurrent;
using Domain.Models.PriceRule;

namespace Application.Rules;

public class RuleCache
{
    private readonly ConcurrentDictionary<long, PriceRule> _rules = new ConcurrentDictionary<long, PriceRule>();

    public IEnumerable<PriceRule> GetAllRules()
    {
        return _rules.Values;
    }

    public PriceRule GetRule(int ruleId)
    {
        _rules.TryGetValue(ruleId, out var rule);
        return rule;
    }

    public void AddOrUpdateRule(PriceRule rule)
    {
        _rules.AddOrUpdate(rule.Id, rule, (key, oldValue) => rule);
    }

    public void RemoveRule(int ruleId)
    {
        _rules.TryRemove(ruleId, out _);
    }

    public void LoadRules(IEnumerable<PriceRule> rules)
    {
        _rules.Clear();
        foreach (var rule in rules)
        {
            _rules[rule.Id] = rule;
        }
    }
}