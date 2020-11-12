using System;
using Checkout.Contracts;

namespace Checkout.Presentation.Commands
{
    public class SetBirthDateCommand : ICommand
    {
        private readonly ICheckoutService _service;

        public SetBirthDateCommand(ICheckoutService service)
        {
            _service = service;
        }

        public bool CanExecute => true;
        public DateTime BirthDate { get; set; }

        public void Execute() => _service.SetCustomerBirthDate(BirthDate);
    }
}
