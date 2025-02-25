using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

public class ConsoleInputService : IUserInputService
{
    public CalculateRebateRequest GetRebateRequest()
    {
        var request = new CalculateRebateRequest();

        Console.WriteLine("Please enter a product:");
        request.ProductIdentifier = Console.ReadLine();

        Console.WriteLine("Please enter a rebate:");
        request.RebateIdentifier = Console.ReadLine();

        Console.WriteLine("Please enter an item quantity:");
        var volumeString = Console.ReadLine();
        if (decimal.TryParse(volumeString, out decimal volume))
        {
            request.Volume = volume;
        }
        else
        {
            Console.WriteLine("Invalid volume. Defaulting to 0.");
            request.Volume = 0;
        }

        return request;
    }
}