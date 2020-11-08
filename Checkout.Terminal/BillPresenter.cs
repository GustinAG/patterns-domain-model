using System;
using Checkout.Contracts;
using Checkout.Presentation;

namespace Checkout.Terminal
{
    internal sealed class BillPresenter : IPresenter
    {
        private readonly ICheckoutService _service;

        public BillPresenter(ICheckoutService service)
        {
            _service = service;
        }

        public void WarnLimitExceeded(decimal limit, decimal currentPrice)
        {
            Console.WriteLine();
            Console.WriteLine($"Warning: Your limit has been exceeded (limit: € {limit}, current price: € {currentPrice})");
            Console.WriteLine();
        }

        internal void ShowPartialBill()
        {
            Console.WriteLine("Partial bill so far:");
            Console.WriteLine(GetCurrentBillText());
        }

        internal void ShowClosedBill()
        {
            Console.WriteLine($"{Environment.NewLine}BILL:");
            Console.WriteLine(GetCurrentBillText());
        }

        private string GetCurrentBillText()
        {
            var bill = _service.GetCurrentBill();
            return new BillAppearance(bill).AsText;
        }
    }
}
