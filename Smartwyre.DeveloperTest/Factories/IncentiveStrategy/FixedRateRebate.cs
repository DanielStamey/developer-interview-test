using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Factories;

public class FixedRateRebate : IIncentiveStrategy
{
    public bool IsEligible(Product product, Rebate rebate, CalculateRebateRequest request)
    {
        return product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate)
               && rebate.Percentage > 0
               && product.Price > 0
               && request.Volume > 0;
    }

    public decimal CalculateRebateAmount(Product product, Rebate rebate, CalculateRebateRequest request)
    {
        return product.Price * rebate.Percentage * request.Volume;
    }
}