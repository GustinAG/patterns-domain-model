﻿using System;
using DomainModel.AppService;

namespace DomainModel.Checkout.Terminal
{
    internal static class Program
    {
        private static void Main()
        {
            // See: https://www.codeproject.com/Questions/455766/Euro-symbol-does-not-show-up-in-Console-WriteLine
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Press any key to start checkout process!");
            Console.ReadKey(true);

            var service = new CheckoutService();
            var displayer = new BillDisplayer(service);
            var processor = new CommandProcessor(service, displayer);

            service.Start(RenderLimitExceededText);

            string code;

            do
            {
                code = CommandReader.ReadCommandCode();
                processor.Process(code);
            } while (code != CommandCode.Exit);

            service.Close();

            Console.WriteLine($"{Environment.NewLine}BILL:");
            Console.WriteLine(service.GetCurrentBill());
        }


        private static void RenderLimitExceededText(decimal limit, decimal currentPrice)
        {
            Console.WriteLine();
            Console.WriteLine($"Warning: Your limit has been exceeded (limit: € {limit}, current price: € {currentPrice})");
            Console.WriteLine();
        }
    }
}
