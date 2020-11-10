using Checkout.Contracts;

namespace Checkout.Presentation
{
    public class ScanCommand : ICommand
    {
        private readonly ICheckoutService _service;

        public ScanCommand(ICheckoutService service)
        {
            _service = service;
        }

        public string Code { get; set; }

        public bool CanExecute => _service.CanScan;

        public void Execute() => _service.Scan(Code);
    }
}
