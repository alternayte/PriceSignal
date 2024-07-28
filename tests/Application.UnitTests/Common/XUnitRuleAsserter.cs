using NRules.Testing;
using Xunit.Sdk;

namespace Application.UnitTests.Common;

public class XUnitRuleAsserter : IRuleAsserter
{
    public void Assert(RuleAssertResult result)
    {
        if (result.Status == RuleAssertStatus.Failed)
        {
            throw new XunitException(result.GetMessage());
        }
    }
}