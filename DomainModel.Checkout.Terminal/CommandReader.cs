using System;

namespace DomainModel.Checkout.Terminal
{
    internal static class CommandReader
    {
        public static string ReadCommandCode()
        {
            Console.Write($"Bar code - or '{CommandCode.Exit}' to close checkout / '{CommandCode.Show}' to show bill so far / '{CommandCode.Cancel} to cancel one item' / '{CommandCode.Limit}' to set up a total price limit: ");
            return Console.ReadLine();
        }
    }
}
