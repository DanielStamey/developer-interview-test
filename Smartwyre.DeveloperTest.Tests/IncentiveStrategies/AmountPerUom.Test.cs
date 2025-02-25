using Xunit;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class AmountPerUomTests
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
    Incentive = IncentiveType.AmountPerUom,
    Amount = 10.00M,
    Percentage = 3.00M,
  };
  Product product = new Product()
  {
    Id = 0,
    Identifier = "ProductIdentifier",
    Price = 5.00M,
    Uom = "Each",
    SupportedIncentives = SupportedIncentiveType.AmountPerUom,
  };

  IIncentiveStrategy amountPerUom = new AmountPerUom();

  [Theory]
  [InlineData(10, 5, true, SupportedIncentiveType.AmountPerUom)]
  [InlineData(10, 5, false, SupportedIncentiveType.FixedCashAmount)]
  [InlineData(0, 5, false, SupportedIncentiveType.AmountPerUom)]
  [InlineData(10, 0, false, SupportedIncentiveType.AmountPerUom)]
  public void IsEligible_ReturnsCorrectBool(decimal amount,
    decimal volume,
    bool expectedAnswer,
    SupportedIncentiveType supportedIncentiveType)
  {
    calculateRebateRequest.Volume = volume;
    rebate.Amount = amount;
    product.SupportedIncentives = supportedIncentiveType;

    var result = amountPerUom.IsEligible(product, rebate, calculateRebateRequest);

    Assert.Equal(expectedAnswer, result);
  }

  [Theory]
  [InlineData(10, 5, 50)]
  [InlineData(10, 2, 20)]
  [InlineData(20, 5, 100)]
  [InlineData(0.20, 5, 1)]
  public void CalculateRebateAmount_ReturnsCorrectAmount(decimal amount,
    decimal volume,
    decimal expectedAnswer)
  {
    calculateRebateRequest.Volume = volume;
    rebate.Amount = amount;

    var result = amountPerUom.CalculateRebateAmount(product, rebate, calculateRebateRequest);

    Assert.Equal(expectedAnswer, result);
  }

  [Fact]
  public void CalculateRebate_WithValidData_ReturnsCorrectResults()
  {
    var expected = new CalculateRebateResult()
    {
      Success = true,
      Amount = 20,
    };

    var result = amountPerUom.CalculateRebate(product, rebate, calculateRebateRequest);

    Assert.Equal(expected.Success, result.Success);
    Assert.Equal(expected.Amount, result.Amount);
  }

  [Fact]
  public void CalculateRebate_WithNullProduct_ReturnsSuccessOfFalse()
  {
    var result = amountPerUom.CalculateRebate(null, rebate, calculateRebateRequest);

    Assert.False(result.Success);
  }

  [Fact]
  public void CalculateRebate_WithNullRebate_ReturnsSuccessOfFalse()
  {
    var result = amountPerUom.CalculateRebate(product, null, calculateRebateRequest);

    Assert.False(result.Success);
  }

  [Fact]
  public void CalculateRebate_WithNullRebateRequest_ReturnsSuccessOfFalse()
  {
    var result = amountPerUom.CalculateRebate(product, rebate, null);

    Assert.False(result.Success);
  }
}