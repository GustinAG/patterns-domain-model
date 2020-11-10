using Checkout.Contracts;

namespace Checkout.Presentation
{
    public class CancelCommand : ICommand
    {
        private readonly ICheckoutService _service;
       
        public CancelCommand(ICheckoutService service)
        {
            _service = service;
        }

        public bool CanExecute => _service.CanCancel;

        public string Code { get; set; }

        public void Execute() => _service.Cancel(Code);
    }
}
