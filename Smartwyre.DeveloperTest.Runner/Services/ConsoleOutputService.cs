using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

public class ConsoleOutputService : IOutputService
{
    public void DisplayResult(CalculateRebateResult result)
    {
        var status = result.Success ? "succeeded" : "failed";
        Console.WriteLine($"Your rebate request {status}.");
        if (result.Success)
        {
            Console.WriteLine($"You saved {result.Amount.ToString("C")}.");
        }

        Console.WriteLine("App Ended");
        Console.ReadLine();
    }
}