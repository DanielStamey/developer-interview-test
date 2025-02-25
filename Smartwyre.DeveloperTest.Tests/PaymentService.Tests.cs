using Xunit;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Moq;
using Smartwyre.DeveloperTest.Data;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
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

    [Theory]
    [InlineData(IncentiveType.FixedCashAmount, 10, 5, 3, 2, 10, SupportedIncentiveType.FixedCashAmount)]
    [InlineData(IncentiveType.FixedRateRebate, 10, 5, 3, 2, 30, SupportedIncentiveType.FixedRateRebate)]
    [InlineData(IncentiveType.AmountPerUom, 10, 5, 3, 2, 20, SupportedIncentiveType.AmountPerUom)]
    public void Calculate_WithHappyPath_ShouldCalculateCorrectly(IncentiveType incentiveType,
      decimal amount,
      decimal price,
      decimal percent,
      decimal volume,
      decimal total,
      SupportedIncentiveType supportedIncentiveType)
    {
      calculateRebateRequest.Volume = volume;
      rebate.Incentive = incentiveType;
      rebate.Amount = amount;
      rebate.Percentage = percent;
      product.Price = price;
      product.SupportedIncentives = supportedIncentiveType;

      var mockRebateDataStore = new Mock<IRebateDataStore>();
      mockRebateDataStore.Setup(x => x.GetRebate(calculateRebateRequest.RebateIdentifier)).Returns(rebate);

      var mockProductDataStore = new Mock<IProductDataStore>();
      mockProductDataStore.Setup(x => x.GetProduct(product.Identifier)).Returns(product);

      RebateService rebateService = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

      var result = rebateService.Calculate(calculateRebateRequest);

      Assert.True(result.Success);
      mockRebateDataStore.Verify(x => x.StoreCalculationResult(rebate, total), Times.Once());
    }

    [Theory]
    [InlineData(IncentiveType.FixedCashAmount, 10, 5, 3, 2, SupportedIncentiveType.AmountPerUom)]
    [InlineData(IncentiveType.FixedRateRebate, 10, 5, 3, 2, SupportedIncentiveType.FixedCashAmount)]
    [InlineData(IncentiveType.AmountPerUom, 10, 5, 3, 2, SupportedIncentiveType.FixedRateRebate)]
    [InlineData(IncentiveType.FixedCashAmount, 0, 5, 3, 2, SupportedIncentiveType.FixedCashAmount)]
    [InlineData(IncentiveType.FixedRateRebate, 10, 5, 0, 2, SupportedIncentiveType.FixedRateRebate)]
    [InlineData(IncentiveType.FixedRateRebate, 10, 0, 3, 2, SupportedIncentiveType.FixedRateRebate)]
    [InlineData(IncentiveType.FixedRateRebate, 10, 5, 3, 0, SupportedIncentiveType.FixedRateRebate)]
    [InlineData(IncentiveType.AmountPerUom, 10, 5, 3, 0, SupportedIncentiveType.AmountPerUom)]
    [InlineData(IncentiveType.AmountPerUom, 0, 5, 3, 2, SupportedIncentiveType.AmountPerUom)]
    public void Calculate_WithInvalidData_ReturnsFalse(IncentiveType incentiveType,
      decimal amount,
      decimal price,
      decimal percent,
      decimal volume,
      SupportedIncentiveType supportedIncentiveType)
    {
      calculateRebateRequest.Volume = volume;
      rebate.Incentive = incentiveType;
      rebate.Amount = amount;
      rebate.Percentage = percent;
      product.Price = price;
      product.SupportedIncentives = supportedIncentiveType;

      var mockRebateDataStore = new Mock<IRebateDataStore>();
      mockRebateDataStore.Setup(x => x.GetRebate(calculateRebateRequest.RebateIdentifier)).Returns(rebate);

      var mockProductDataStore = new Mock<IProductDataStore>();
      mockProductDataStore.Setup(x => x.GetProduct(product.Identifier)).Returns(product);

      RebateService rebateService = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

      var result = rebateService.Calculate(calculateRebateRequest);

      Assert.False(result.Success);
      mockRebateDataStore
        .Verify(x => 
          x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never());
    }

    [Fact]
    public void Calculate_WithNullRebate_ReturnsFalse()
    {
      var mockRebateDataStore = new Mock<IRebateDataStore>();
      mockRebateDataStore.Setup(x => x.GetRebate(calculateRebateRequest.RebateIdentifier)).Returns<Rebate>(null);

      var mockProductDataStore = new Mock<IProductDataStore>();
      mockProductDataStore.Setup(x => x.GetProduct(product.Identifier)).Returns(product);

      RebateService rebateService = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

      var result = rebateService.Calculate(calculateRebateRequest);

      Assert.False(result.Success);
      mockRebateDataStore
        .Verify(x => 
          x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never());
    }

    [Fact]
    public void Calculate_WithNullProduct_ReturnsFalse()
    {
      var mockRebateDataStore = new Mock<IRebateDataStore>();
      mockRebateDataStore.Setup(x => x.GetRebate(calculateRebateRequest.RebateIdentifier)).Returns(rebate);

      var mockProductDataStore = new Mock<IProductDataStore>();
      mockProductDataStore.Setup(x => x.GetProduct(product.Identifier)).Returns<Product>(null);

      RebateService rebateService = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

      var result = rebateService.Calculate(calculateRebateRequest);

      Assert.False(result.Success);
      mockRebateDataStore
        .Verify(x => 
          x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never());
    }
}
