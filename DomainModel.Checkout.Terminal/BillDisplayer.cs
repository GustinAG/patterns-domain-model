using System;
using DomainModel.AppService;

namespace DomainModel.Checkout.Terminal
{
    internal sealed class BillDisplayer
    {
        private readonly CheckoutService _service;

        internal BillDisplayer(CheckoutService service)
        {
            _service = service;
        }

        internal void ShowPartialBill()
        {
            Console.WriteLine("Partial bill so far:");
            Console.WriteLine(_service.GetCurrentBill());
        }
    }
}
