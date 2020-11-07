using System;
using DomainModel.AppService;

namespace DomainModel.Checkout.Terminal
{
    internal sealed class BillPresenter
    {
        private readonly CheckoutService _service;

        internal BillPresenter(CheckoutService service)
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
