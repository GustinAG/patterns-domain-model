using System;
using Checkout.Contracts;

namespace Checkout.Presentation
{
    public sealed class StartCommand : ICommand
    {
        private readonly ICheckoutService _service;
        private readonly Action<decimal, decimal> _limitExceededAction;

        public StartCommand(IPresenter presenter, ICheckoutService service)
        {
            _service = service;
            _limitExceededAction = (l, p) => presenter.ShowWarning($"Warning: Your limit has been exceeded (limit: € {l}, current price: € {p})");
        }

        public bool CanExecute => _service.CanStart;

        public void Execute() => _service.Start(_limitExceededAction);
    }
}
