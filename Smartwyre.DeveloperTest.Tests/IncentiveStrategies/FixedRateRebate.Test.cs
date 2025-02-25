using Xunit;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class FixedRateRebateTests
{
  CalculateRebateRequest calculateRebateRequest = new CalculateRebateRequest()
  {
    ProductIdentifier = "ProductIdentifier",
    RebateIdentifier = "RebateIdentifier",
    Volume = 2.00M,
  };
  Rebate rebate = new Rebate()
  {
    Identifier = "RebateIdentifier",
    Incentive = IncentiveType.FixedRateRebate,
    Amount = 10.00M,
    Percentage = 3.00M,
  };
  Product product = new Product()
  {
    Id = 0,
    Identifier = "ProductIdentifier",
    Price = 5.00M,
    Uom = "Each",
    SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
  };

  IIncentiveStrategy fixedRateRebate = new FixedRateRebate();

  [Theory]
  [InlineData(10, 5, 3, true, SupportedIncentiveType.FixedRateRebate)]
  [InlineData(10, 5, 3, false, SupportedIncentiveType.AmountPerUom)]
  [InlineData(0, 5, 3, false, SupportedIncentiveType.FixedRateRebate)]
  [InlineData(10, 0, 3, false, SupportedIncentiveType.FixedRateRebate)]
  [InlineData(10, 5, 0, false, SupportedIncentiveType.FixedRateRebate)]
  public void IsEligible_ReturnsCorrectBool(decimal percent,
    decimal price,
    decimal volume,
    bool expectedAnswer,
    SupportedIncentiveType supportedIncentiveType)
  {
    rebate.Percentage = percent;
    product.Price = price;
    calculateRebateRequest.Volume = volume;
    product.SupportedIncentives = supportedIncentiveType;

    var result = fixedRateRebate.IsEligible(product, rebate, calculateRebateRequest);

    Assert.Equal(expectedAnswer, result);
  }

  [Theory]
  [InlineData(2, 4, 6, 48)]
  [InlineData(0.2, 4, 6, 4.8)]
  [InlineData(2, 4, 0.6, 4.8)]
  [InlineData(2, 0.4, 6, 4.8)]
  public void CalculateRebateAmount_ReturnsCorrectAmount(decimal percent,
    decimal price,
    decimal volume,
    decimal expectedAnswer)
  {
    rebate.Percentage = percent;
    product.Price = price;
    calculateRebateRequest.Volume = volume;

    var result = fixedRateRebate.CalculateRebateAmount(product, rebate, calculateRebateRequest);

    Assert.Equal(expectedAnswer, result);
  }

  [Fact]
  public void CalculateRebate_WithValidData_ReturnsCorrectResults()
  {
    var expected = new CalculateRebateResult()
    {
      Success = true,
      Amount = 30,
    };

    var result = fixedRateRebate.CalculateRebate(product, rebate, calculateRebateRequest);

    Assert.Equal(expected.Success, result.Success);
    Assert.Equal(expected.Amount, result.Amount);
  }

  [Fact]
  public void CalculateRebate_WithNullProduct_ReturnsSuccessOfFalse()
  {
    var result = fixedRateRebate.CalculateRebate(null, rebate, calculateRebateRequest);

    Assert.False(result.Success);
  }

  [Fact]
  public void CalculateRebate_WithNullRebate_ReturnsSuccessOfFalse()
  {
    var result = fixedRateRebate.CalculateRebate(product, null, calculateRebateRequest);

    Assert.False(result.Success);
  }

  [Fact]
  public void CalculateRebate_WithNullRebateRequest_ReturnsSuccessOfFalse()
  {
    var result = fixedRateRebate.CalculateRebate(product, rebate, null);

    Assert.False(result.Success);
  }
}