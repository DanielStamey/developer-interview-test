using System.Diagnostics;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    public Rebate GetRebate(string rebateIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        //return new Rebate();

        // Code added for Console App
        switch(rebateIdentifier)
        {
          case "Mail In":
            return new Rebate()
            {
              Identifier = "Mail In",
              Incentive = IncentiveType.FixedCashAmount,
              Amount = 99.99M,
              Percentage = 0,
            };
          case "Bulk":
            return new Rebate()
            {
              Identifier = "Bulk",
              Incentive = IncentiveType.AmountPerUom,
              Amount = 0.05M,
              Percentage = 0,
            };
          case "clearance":
            return new Rebate()
            {
              Identifier = "clearance",
              Incentive = IncentiveType.FixedRateRebate,
              Amount = 0,
              Percentage = 0.10M,
            };
          default:
            return new Rebate();
        }
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
