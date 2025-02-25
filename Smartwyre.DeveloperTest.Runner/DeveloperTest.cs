using Smartwyre.DeveloperTest.Services;

namespace Smartwyre.DeveloperTest.Runner;

public class App
{
    private readonly IRebateService _rebateService;
    private readonly IUserInputService _inputService;
    private readonly IOutputService _outputService;

    public App(
        IRebateService rebateService,
        IUserInputService inputService,
        IOutputService outputService)
    {
        _rebateService = rebateService;
        _inputService = inputService;
        _outputService = outputService;
    }

    public void Run()
    {
        var request = _inputService.GetRebateRequest();
        var result = _rebateService.Calculate(request);
        _outputService.DisplayResult(result);
    }
}