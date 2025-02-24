using System;
using Xunit;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Moq;
using Smartwyre.DeveloperTest.Data;
using System.Diagnostics.Contracts;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    [Fact]
    public void Happy_Path()
    {
      CalculateRebateRequest calculateRebateRequest= new CalculateRebateRequest();
      calculateRebateRequest.ProductIdentifier = "ProductIdentifier";
      calculateRebateRequest.RebateIdentifier = "RebateIdentifier";
      calculateRebateRequest.Volume = 0;

      var rebate = new Rebate();
      rebate.Identifier = "RebateIdentifier";
      rebate.Incentive = IncentiveType.FixedCashAmount;
      rebate.Amount = 5;
      rebate.Percentage = 0;

      var mockRebateDataStore = new Mock<IRebateDataStore>();
      mockRebateDataStore.Setup(x => x.GetRebate(calculateRebateRequest.RebateIdentifier)).Returns(rebate);

      var product = new Product();
      product.Id = 0;
      product.Identifier = "ProductIdentifier";
      product.Price = 0;
      product.Uom = "Uom";
      product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;

      var mockProductDataStore = new Mock<IProductDataStore>();
      mockProductDataStore.Setup(x => x.GetProduct(product.Identifier)).Returns(product);

      RebateService rebateService = new RebateService(mockRebateDataStore.Object, mockProductDataStore.Object);

      var result = rebateService.Calculate(calculateRebateRequest);

      Assert.True(result.Success);
      mockRebateDataStore.Verify(x => x.StoreCalculationResult(rebate, rebate.Amount), Times.Once());
    }
}
