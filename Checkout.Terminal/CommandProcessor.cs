using Checkout.Contracts;
using Checkout.Presentation;

namespace Checkout.Terminal
{
    internal sealed class CommandProcessor
    {
        private readonly ICommandReader _commandReader;
        private readonly Invoker _invoker;
        private readonly StartCommand _startCommand;
        private readonly ScanCommand _scanCommand;
        private readonly CancelCommand _cancelCommand;
        private readonly StopCommand _stopCommand;
        private readonly ICheckoutService _service;

        public CommandProcessor(ICommandReader commandReader, Invoker invoker, StartCommand startCommand, ScanCommand scanCommand, CancelCommand cancelCommand, StopCommand stopCommand, ICheckoutService service)
        {
            _commandReader = commandReader;
            _invoker = invoker;
            _startCommand = startCommand;
            _scanCommand = scanCommand;
            _cancelCommand = cancelCommand;
            _stopCommand = stopCommand;
            _service = service;
        }

        internal void Start() => _invoker.Invoke(_startCommand);

        internal void Process(string code)
        {
            switch (code)
            {
                case CommandCode.Exit:
                    _invoker.Invoke(_stopCommand);
                    break;
                case CommandCode.Limit:
                    SetUpPriceLimit();
                    break;
                case CommandCode.Cancel:
                    _cancelCommand.Code = _commandReader.ReadCancelBarCode();
                    _invoker.Invoke(_cancelCommand);
                    break;
                default:
                    _scanCommand.Code = code;
                    _invoker.Invoke(_scanCommand);
                    break;
            }
        }

        private void SetUpPriceLimit()
        {
            var limit = _commandReader.ReadPriceLimit();
            _service.SetUpLimit(limit);
        }
    }
}
