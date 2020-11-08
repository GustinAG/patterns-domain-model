using System;
using Checkout.Contracts;
using Checkout.Domain.Checkout;
using Checkout.Presentation;

namespace Checkout.Terminal
{
    internal sealed class CommandProcessor
    {
        private readonly ICommandReader _commandReader;
        private readonly StartCommand _startCommand;
        private readonly ICheckoutService _service;
        private readonly BillPresenter _presenter;

        public CommandProcessor(ICommandReader commandReader, StartCommand startCommand, ICheckoutService service, BillPresenter presenter)
        {
            _commandReader = commandReader;
            _startCommand = startCommand;
            _service = service;
            _presenter = presenter;
        }

        internal void Start() => _startCommand.Execute();

        internal void Process(string code)
        {
            switch (code)
            {
                case CommandCode.Exit:
                    break;
                case CommandCode.Show:
                    _presenter.ShowPartialBill();
                    break;
                case CommandCode.Limit:
                    SetUpPriceLimit();
                    break;
                case CommandCode.Cancel:
                    RunWithCheckoutExceptionHandling(CancelItem);
                    break;
                default:
                    ScanItem(code);
                    break;
            }
        }

        private void ScanItem(string code)
        {
            try
            {
                _service.Scan(code);
                Console.WriteLine(_service.GetLastAdded());
            }
            catch (Exception e)
                when (e is InvalidBarCodeException || e is BoughtProductNotFoundException)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void CancelItem()
        {
            var code = _commandReader.ReadCancelBarCode();
            _service.Cancel(code);
            _presenter.ShowPartialBill();
        }

        private void SetUpPriceLimit()
        {
            var limit = _commandReader.ReadPriceLimit();
            _service.SetUpLimit(limit);
        }

        private static void RunWithCheckoutExceptionHandling(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
                when (e is InvalidBarCodeException || e is BoughtProductNotFoundException)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
