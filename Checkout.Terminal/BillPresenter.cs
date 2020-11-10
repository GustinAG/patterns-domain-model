using System;
using Checkout.Presentation;

namespace Checkout.Terminal
{
    internal sealed class BillPresenter : IPresenter
    {
        public void RefreshDisplay(BillAppearance appearance)
        {
            Console.WriteLine();
            Console.WriteLine("BILL:");
            Console.WriteLine(appearance.AsText);
        }

        public void ShowWarning(string message) => ShowImportantMessage(message);

        public void ShowError(string message) => ShowImportantMessage(message);

        private static void ShowImportantMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.WriteLine();
        }
    }
}
