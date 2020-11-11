using System;
using Checkout.Contracts;
using Checkout.Domain.Checkout;

namespace Checkout.Presentation.Commands
{
    public sealed class StartCommand : ICommand
    {
        private readonly ICheckoutService _service;
        private readonly Action<CheckoutLimitExceeded> _limitExceededAction;

        public StartCommand(IPresenter presenter, ICheckoutService service)
        {
            _service = service;
            _limitExceededAction = e => presenter.ShowWarning($"Warning: Your limit has been exceeded (limit: € {e.Limit}, current price: € {e.Price})");
        }

        public bool CanExecute => _service.CanStart;

        public void Execute() => _service.Start(_limitExceededAction);
    }
}
