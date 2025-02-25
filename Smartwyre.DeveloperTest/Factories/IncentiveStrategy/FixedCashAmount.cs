using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Factories;

public class FixedCashAmount : IIncentiveStrategy
{
    public bool IsEligible(Product product, Rebate rebate, CalculateRebateRequest request)
    {
        return product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount)
               && rebate.Amount > 0;
    }

    public decimal CalculateRebateAmount(Product product, Rebate rebate, CalculateRebateRequest request)
    {
        return rebate.Amount;
    }
}