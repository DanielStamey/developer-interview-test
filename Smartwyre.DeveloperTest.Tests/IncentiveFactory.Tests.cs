using System;
using Xunit;

using Smartwyre.DeveloperTest.Factories;

namespace Smartwyre.DeveloperTest.Tests;

public class IncentiveFactoryTests
{
    IncentiveStrategyFactory factory = new IncentiveStrategyFactory();

    [Theory]
    [InlineData("FixedCashAmount", typeof(FixedCashAmount))]
    [InlineData("FixedRateRebate", typeof(FixedRateRebate))]
    [InlineData("AmountPerUom", typeof(AmountPerUom))]
    public void GetIncentiveStrategy_ReturnsCorretStrategy(string incentiveName, Type incentiveType)
    {
      var incentive = factory.GetIncentiveStrategy(incentiveName);

      Assert.IsType(incentiveType, incentive);
    }

    [Fact]
    public void GetIncentiveStrategy_WithInvalidIncentive_ThrowsArgumentException()
    {
      Assert.Throws<ArgumentException>(() => factory.GetIncentiveStrategy("InvalidType"));
    }
}
