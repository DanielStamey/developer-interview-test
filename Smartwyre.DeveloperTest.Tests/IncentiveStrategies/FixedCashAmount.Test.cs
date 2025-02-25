using Xunit;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class FixedCashAmountTests
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
    Incentive = IncentiveType.FixedCashAmount,
    Amount = 10.00M,
    Percentage = 3.00M,
  };
  Product product = new Product()
  {
    Id = 0,
    Identifier = "ProductIdentifier",
    Price = 5.00M,
    Uom = "Each",
    SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
  };

  IIncentiveStrategy fixedCashAmount = new FixedCashAmount();

  [Theory]
  [InlineData(10, true, SupportedIncentiveType.FixedCashAmount)]
  public void IsEligible_ReturnsCorrectBool(decimal amount,
    bool expectedAnswer,
    SupportedIncentiveType supportedIncentiveType)
  {
    rebate.Amount = amount;
    product.SupportedIncentives = supportedIncentiveType;

    var result = fixedCashAmount.IsEligible(product, rebate, calculateRebateRequest);

    Assert.Equal(expectedAnswer, result);
  }

  [Theory]
  [InlineData(10)]
  [InlineData(55)]
  [InlineData(15.5)]
  public void CalculateRebateAmount_ReturnsCorrectAmount(decimal amount)
  {
    rebate.Amount = amount;

    var result = fixedCashAmount.CalculateRebateAmount(product, rebate, calculateRebateRequest);

    Assert.Equal(amount, result);
  }

  [Fact]
  public void CalculateRebate_WithValidData_ReturnsCorrectResults()
  {
    var expected = new CalculateRebateResult()
    {
      Success = true,
      Amount = 10,
    };

    var result = fixedCashAmount.CalculateRebate(product, rebate, calculateRebateRequest);

    Assert.Equal(expected.Success, result.Success);
    Assert.Equal(expected.Amount, result.Amount);
  }

  [Fact]
  public void CalculateRebate_WithNullProduct_ReturnsSuccessOfFalse()
  {
    var result = fixedCashAmount.CalculateRebate(null, rebate, calculateRebateRequest);

    Assert.False(result.Success);
  }

  [Fact]
  public void CalculateRebate_WithNullRebate_ReturnsSuccessOfFalse()
  {
    var result = fixedCashAmount.CalculateRebate(product, null, calculateRebateRequest);

    Assert.False(result.Success);
  }

  [Fact]
  public void CalculateRebate_WithNullRebateRequest_ReturnsSuccessOfFalse()
  {
    var result = fixedCashAmount.CalculateRebate(product, rebate, null);

    Assert.False(result.Success);
  }
}