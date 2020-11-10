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
        private readonly SetLimitCommand _setLimitCommand;
        private readonly StopCommand _stopCommand;

        public CommandProcessor(ICommandReader commandReader, Invoker invoker, StartCommand startCommand, ScanCommand scanCommand, CancelCommand cancelCommand, SetLimitCommand setLimitCommand, StopCommand stopCommand)
        {
            _commandReader = commandReader;
            _invoker = invoker;
            _startCommand = startCommand;
            _scanCommand = scanCommand;
            _cancelCommand = cancelCommand;
            _setLimitCommand = setLimitCommand;
            _stopCommand = stopCommand;
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
                    _setLimitCommand.Limit = _commandReader.ReadPriceLimit();
                    _invoker.Invoke(_setLimitCommand);
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
    }
}
