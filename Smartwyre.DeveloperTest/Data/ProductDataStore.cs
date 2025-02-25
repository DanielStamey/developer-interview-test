using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    public Product GetProduct(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        //  return new Product();

        // Code added for Console App
        switch(productIdentifier)
        {
          case "Laptop":
            return new Product()
            {
              Id = 1,
              Identifier = "Laptop",
              Price = 1549.95M,
              Uom = "Each",
              SupportedIncentives = SupportedIncentiveType.FixedCashAmount
            };
          case "Candy":
            return new Product()
            {
              Id = 1,
              Identifier = "Candy",
              Price = 1.00M,
              Uom = "10",
              SupportedIncentives = SupportedIncentiveType.AmountPerUom
            };
          case "Shirt":
            return new Product()
            {
              Id = 1,
              Identifier = "Shirt",
              Price = 30.00M,
              Uom = "Each",
              SupportedIncentives = SupportedIncentiveType.FixedRateRebate
            };
          default:
            return new Product();
        }
    }
}
