using Checkout.Contracts;

namespace Checkout.Presentation.Commands
{
    public sealed class StartCommand : ICommand
    {
        private readonly ICheckoutService _service;

        public StartCommand(ICheckoutService service)
        {
            _service = service;
        }

        public bool CanExecute => _service.CanStart;

        public void Execute() => _service.Start();
    }
}
