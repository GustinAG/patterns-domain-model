using System;
using DomainModel.AppService;
using DomainModel.Domain.Checkout;

namespace DomainModel.Checkout.Terminal
{
    internal static class Program
    {
        private const string ExitCode = "c";
        private const string ShowCode = "s";
        private const string CancelCode = "r";
        private const string LimitCode = "l";
        private static CheckoutService Service;

        private static void Main()
        {
            // See: https://www.codeproject.com/Questions/455766/Euro-symbol-does-not-show-up-in-Console-WriteLine
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Press any key to start checkout process!");
            Console.ReadKey(true);

            Service = new CheckoutService();
            Service.Start(RenderLimitExceededText);

            string code;

            do
            {
                Console.Write($"Bar code - or '{ExitCode}' to close checkout / '{ShowCode}' to show bill so far / '{CancelCode} to cancel one item' / '{LimitCode}' to set up a total price limit: ");
                code = Console.ReadLine();
                if (code == ExitCode) continue;

                if (code == ShowCode)
                {
                    ShowPartialBill();
                    continue;
                }

                if (code == LimitCode)
                {
                    SetUpPriceLimit();
                    continue;
                }

                try
                {
                    if (code == CancelCode)
                    {
                        CancelItem();
                        continue;
                    }

                    Service.Scan(code);
                    Console.WriteLine(Service.GetLastAdded());
                }
                catch (Exception e)
                    when (e is InvalidBarCodeException || e is BoughtProductNotFoundException)
                {
                    Console.WriteLine(e.Message);
                }
            } while (code != ExitCode);

            Service.Close();

            Console.WriteLine($"{Environment.NewLine}BILL:");
            Console.WriteLine(Service.GetCurrentBill());
        }

        private static void RenderLimitExceededText(decimal limit, decimal currentPrice)
        {
            Console.WriteLine($"Warning: Your limit has been exceeded (limit: € {limit}, current price: € {currentPrice})");
        }

        private static void SetUpPriceLimit()
        {
            var limit = ReadDecimalFromKeybard("Please enter price limit (0 for no limit): ");
            Service.SetUpLimit(limit);
        }

        private static void ShowPartialBill()
        {
            Console.WriteLine("Partial bill so far:");
            Console.WriteLine(Service.GetCurrentBill());
        }

        private static void CancelItem()
        {
            Console.Write("Bar code to cancel: ");
            string code = Console.ReadLine();
            Service.Cancel(code);
            ShowPartialBill();
        }

        private static decimal ReadDecimalFromKeybard(string initialText)
        {
            Console.Write(initialText);
            string numberAsText = Console.ReadLine();
            decimal number;
            while (!decimal.TryParse(numberAsText, out number))
            {
                Console.Write("The text you entered isn't a valid number. Please try again: ");
                numberAsText = Console.ReadLine();
            }

            return number;
        }
    }
}
