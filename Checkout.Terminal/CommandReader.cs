using System;

namespace Checkout.Terminal
{
    internal  class CommandReader : ICommandReader
    {
        public string ReadCommandCode()
        {
            Console.Write($"Bar code - or '{CommandCode.Exit}' to close checkout / '{CommandCode.Show}' to show bill so far / '{CommandCode.Cancel} to cancel one item' / '{CommandCode.Limit}' to set up a total price limit: ");
            return Console.ReadLine();
        }

        public decimal ReadPriceLimit()
        {
            Console.Write("Please enter price limit (0 for no limit): ");
            var numberAsText = Console.ReadLine();
            decimal number;

            while (!Decimal.TryParse(numberAsText, out number))
            {
                Console.Write("The text you entered isn't a valid number. Please try again: ");
                numberAsText = Console.ReadLine();
            }

            return number;
        }

        public string ReadCancelBarCode()
        {
            Console.Write("Bar code to cancel: ");
            return Console.ReadLine();
        }
    }
}
