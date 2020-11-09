using Checkout.Contracts;

namespace Checkout.Presentation
{
    public sealed class StopCommand : ICommand
    {
        private readonly ICheckoutService _service;

        public StopCommand(ICheckoutService service)
        {
            _service = service;
        }

        public bool CanExecute => _service.CanClose;

        public void Execute() => _service.Close();
    }
}
