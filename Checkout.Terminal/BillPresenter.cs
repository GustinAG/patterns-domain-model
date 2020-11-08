using System;
using Checkout.Contracts;

namespace Checkout.Terminal
{
    internal sealed class BillPresenter
    {
        private readonly ICheckoutService _service;

        public BillPresenter(ICheckoutService service)
        {
            _service = service;
        }

        internal void ShowPartialBill()
        {
            Console.WriteLine("Partial bill so far:");
            Console.WriteLine(_service.GetCurrentBill());
        }

        internal void ShowClosedBill()
        {
            Console.WriteLine($"{Environment.NewLine}BILL:");
            Console.WriteLine(_service.GetCurrentBill());
        }
    }
}
