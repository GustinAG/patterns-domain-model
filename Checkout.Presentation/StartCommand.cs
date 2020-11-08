using Checkout.Contracts;

namespace Checkout.Presentation
{
    public class StartCommand : ICommand
    {
        private readonly IPresenter _presenter;
        private readonly ICheckoutService _service;

        public StartCommand(IPresenter presenter, ICheckoutService service)
        {
            _presenter = presenter;
            _service = service;
        }

        public bool CanExecute => _service.CanStart;

        public void Execute() => _service.Start(_presenter.WarnLimitExceeded);
    }
}
