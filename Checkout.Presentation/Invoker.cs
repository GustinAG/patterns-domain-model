using System;
using Checkout.Contracts;
using Checkout.Domain.Checkout;

namespace Checkout.Presentation
{
    public class Invoker
    {
        private readonly IPresenter _presenter;
        private readonly ICheckoutService _service;

        public Invoker(IPresenter presenter, ICheckoutService service)
        {
            _presenter = presenter;
            _service = service;
        }

        public void Invoke(ICommand command)
        {
            try
            {
                command.Execute();
                RefreshDisplay();
            }
            catch (Exception e)
                when (e is InvalidBarCodeException || e is BoughtProductNotFoundException)
            {
                _presenter.ShowError(e.Message);
            }
        }

        private void RefreshDisplay()
        {
            var bill = _service.GetCurrentBill();
            var appearance = new BillAppearance(bill);
            _presenter.RefreshDisplay(appearance);
        }
    }
}
