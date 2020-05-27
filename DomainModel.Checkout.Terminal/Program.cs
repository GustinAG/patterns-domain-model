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
        private static CheckoutService _service;

        private static void Main()
        {
            // See: https://www.codeproject.com/Questions/455766/Euro-symbol-does-not-show-up-in-Console-WriteLine
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Press any key to start checkout process!");
            Console.ReadKey(true);

            _service = new CheckoutService();
            _service.Start();

            string code;

            do
            {
                Console.Write($"Bar code - or '{ExitCode}' to close checkout / '{ShowCode}' to show bill so far / '{CancelCode} to cancel one item': ");
                code = Console.ReadLine();
                if (code == ExitCode) continue;

                if (code == ShowCode)
                {
                    ShowPartialBill();
                    continue;
                }

                try
                {
                    if (code == CancelCode)
                    {
                        CancelItem();
                        continue;
                    }

                    _service.Scan(code);
                    Console.WriteLine(_service.GetLastAdded());
                }
                catch (Exception e)
                    when (e is InvalidBarCodeException || e is BoughtProductNotFoundException)
                {
                    Console.WriteLine(e.Message);
                }
            } while (code != ExitCode);

            _service.Close();

            Console.WriteLine($"{Environment.NewLine}BILL:");
            Console.WriteLine(_service.GetCurrentBill());
        }

        private static void ShowPartialBill()
        {
            Console.WriteLine("Partial bill so far:");
            Console.WriteLine(_service.GetCurrentBill());
        }

        private static void CancelItem()
        {
            Console.Write("Bar code to cancel: ");
            string code = Console.ReadLine();
            _service.Cancel(code);
            ShowPartialBill();
        }
    }
}
