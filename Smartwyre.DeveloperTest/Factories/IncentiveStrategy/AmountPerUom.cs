using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Factories;

public class AmountPerUom : IIncentiveStrategy
{
  public bool IsEligible(Product product, Rebate rebate, CalculateRebateRequest request)
  {
    return product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom)
           && rebate.Amount > 0
           && request.Volume > 0;
  }

  public decimal CalculateRebateAmount(Product product, Rebate rebate, CalculateRebateRequest request)
  {
    return rebate.Amount * request.Volume;
  }

}