using System;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Factories;

public class IncentiveStrategyFactory
{
    private readonly Dictionary<string, Func<IIncentiveStrategy>> _incentiveStrategyCreators;

    public IncentiveStrategyFactory()
    {
        _incentiveStrategyCreators = new Dictionary<string, Func<IIncentiveStrategy>>()
        {
            { "fixedcashamount", () => new FixedCashAmount() },
            { "fixedraterebate", () => new FixedRateRebate() },
            { "amountperuom", () => new AmountPerUom() },
        };
    }

    public IIncentiveStrategy GetIncentiveStrategy(string incentiveType)
    {
        if (_incentiveStrategyCreators.ContainsKey(incentiveType.ToLower()))
        {
            return _incentiveStrategyCreators[incentiveType.ToLower()]();
        }

        throw new ArgumentException("Invalid incentive type");
    }
}