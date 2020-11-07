using System;
using DomainModel.Contracts;
using DomainModel.Domain.Checkout;

namespace DomainModel.Checkout.Terminal
{
    internal sealed class CommandProcessor
    {
        private readonly ICommandReader _commandReader;
        private readonly ICheckoutService _service;
        private readonly BillPresenter _presenter;

        public CommandProcessor(ICommandReader commandReader, ICheckoutService service, BillPresenter presenter)
        {
            _commandReader = commandReader;
            _service = service;
            _presenter = presenter;
        }

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
