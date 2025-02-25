namespace Smartwyre.DeveloperTest.Factories;

public interface IIncentiveStrategyFactory
{
    public IIncentiveStrategy GetIncentiveStrategy(string incentiveType);
}