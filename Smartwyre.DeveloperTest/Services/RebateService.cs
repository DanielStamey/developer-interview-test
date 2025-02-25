using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IIncentiveStrategyFactory _incentiveStrategyFactory;

    public RebateService(
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore,
        IIncentiveStrategyFactory incentiveStrategyFactory)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _incentiveStrategyFactory = incentiveStrategyFactory;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult() {Success = false};
        if (request == null) return result;

        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = _productDataStore.GetProduct(request.ProductIdentifier);

        if (product == null || rebate == null) return result;

        var strategy = _incentiveStrategyFactory.GetIncentiveStrategy(rebate.Incentive.ToString());
        result = strategy.CalculateRebate(product, rebate, request);

        if (result.Success)
        {
            _rebateDataStore.StoreCalculationResult(rebate, result.Amount);
        }

        return result;
    }
}
