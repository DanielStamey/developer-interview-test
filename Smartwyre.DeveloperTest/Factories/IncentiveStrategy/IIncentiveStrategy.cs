using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Factories;

public interface IIncentiveStrategy
{
    bool IsEligible(Product product, Rebate rebate, CalculateRebateRequest request);
    decimal CalculateRebateAmount(Product product, Rebate rebate, CalculateRebateRequest request);

    public CalculateRebateResult CalculateRebate(Product product, Rebate rebate, CalculateRebateRequest request)
    {
      if (product == null
        || rebate == null
        || request == null
        || !IsEligible(product, rebate, request))
      {
        return new CalculateRebateResult(){Success = false};
      }

      var amount = CalculateRebateAmount(product, rebate, request);
      return new CalculateRebateResult(){Success = true, Amount = amount};
    }
}