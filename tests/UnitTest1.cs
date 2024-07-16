namespace tests;
using NRules.Fluent.Dsl;
using NRules.Testing;
using Xunit.Sdk;

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
public abstract class BaseRulesTestFixture : RulesTestFixture
{
    protected BaseRulesTestFixture()
    {
        Asserter = new XUnitRuleAsserter();
    }
}
public class UnitTest1 : BaseRulesTestFixture
{
    public class FactCustomer {}
    public class FactOrder {}

    public class CustomerOrderRule : Rule
    {
        public override void Define()
        {
            FactCustomer customer = default!;

            When().Match(() => customer!);
            Then().Do(_ => Console.WriteLine("CustomerOrderRule fired"));
        }
    }

    public UnitTest1()
    {
        Setup.Rule<CustomerOrderRule>();
    }

    [Fact]
    public void Test1()
    {
        var customer = new FactCustomer();
        Session.Insert(customer);
        Session.Fire();
        Verify(x => x.Rule().Fired(Times.Once));
    }
}