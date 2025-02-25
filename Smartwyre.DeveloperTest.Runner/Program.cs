using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Factories;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var app = serviceProvider.GetService<App>();
            app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<App>();
        
        services.AddTransient<IUserInputService, ConsoleInputService>();
        services.AddTransient<IOutputService, ConsoleOutputService>();

        services.AddTransient<IRebateService, RebateService>();
        services.AddTransient<IRebateDataStore, RebateDataStore>();
        services.AddTransient<IProductDataStore, ProductDataStore>();
        services.AddTransient<IIncentiveStrategyFactory, IncentiveStrategyFactory>();
}
}
