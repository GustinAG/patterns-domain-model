using Checkout.Contracts;

namespace Checkout.Presentation.Commands
{
    public class SetLimitCommand : ICommand
    {
        private readonly ICheckoutService _service;

        public SetLimitCommand(ICheckoutService service)
        {
            _service = service;
        }

        public bool CanExecute => _service.CanSetUpLimit;

        public decimal Limit { get; set; }

        public void Execute() => _service.SetUpLimit(Limit);
    }
}
