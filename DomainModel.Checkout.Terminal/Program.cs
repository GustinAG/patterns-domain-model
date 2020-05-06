using System;
using DomainModel.AppService;
using DomainModel.Domain.Checkout;

namespace DomainModel.Checkout.Terminal
{
    internal static class Program
    {
        private const string ExitCode = "c";
        private const string SkipCode = "c"; 
        private const string ShowCode = "s";
        private const string StornoCode = "r ";

        private static void Main()
        {
            // See: https://www.codeproject.com/Questions/455766/Euro-symbol-does-not-show-up-in-Console-WriteLine
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Press any key to start checkout process!");
            Console.ReadKey(true);

            var service = new CheckoutService();
            service.Start();

            string code;

            Console.Write($"Set credit limit - or continue '{SkipCode}'");
            code = Console.ReadLine();
            if (code != SkipCode)
            {
                try
                {
                    var rawLimit = Decimal.Parse(code);
                    service.CreditLimit = new Domain.Limiter.CreditLimit
                    {
                        CashLimit = rawLimit
                    };
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            do
            {
                Console.Write($"Bar code (use '{StornoCode}' at the front of barcode) - or '{ExitCode}' to close checkout / '{ShowCode}' to show bill so far: ");
                code = Console.ReadLine();
                if (code == ExitCode) continue;

                if (code == ShowCode)
                {
                    Console.WriteLine("Partial bill so far:");
                    Console.WriteLine(service.GetCurrentBill());
                    continue;
                }

                try
                {
                    if (code.StartsWith(StornoCode))
                    {
                        Console.WriteLine("Storno from:");
                        Console.WriteLine(service.GetCurrentBill());
                        var nakedBarCode = code.Substring(StornoCode.Length);
                        Console.WriteLine(service.Storno(nakedBarCode));
                        Console.WriteLine("To:");
                        Console.WriteLine(service.GetCurrentBill());
                    }
                    else
                    {
                        Console.WriteLine(service.Scan(code));
                    }

                }
                catch (InvalidBarCodeException e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (code != ExitCode);

            service.Close();

            Console.WriteLine($"{Environment.NewLine}BILL:");
            Console.WriteLine(service.GetCurrentBill());
        }
    }
}
