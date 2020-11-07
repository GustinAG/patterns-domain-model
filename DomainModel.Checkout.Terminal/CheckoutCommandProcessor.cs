using System;
using DomainModel.AppService;
using DomainModel.Domain.Checkout;

namespace DomainModel.Checkout.Terminal
{
    internal sealed class CommandProcessor
    {
        private readonly CheckoutService _service;
        private readonly BillDisplayer _displayer;

        internal CommandProcessor(CheckoutService service, BillDisplayer displayer)
        {
            _service = service;
            _displayer = displayer;
        }

        internal void Process(string code)
        {
            switch (code)
            {
                case CommandCode.Exit:
                    break;
                case CommandCode.Show:
                    _displayer.ShowPartialBill();
                    break;
                case CommandCode.Limit:
                    SetUpPriceLimit();
                    break;
                case CommandCode.Cancel:
                    CancelItem();
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
            try
            {
                Console.Write("Bar code to cancel: ");
                string code = Console.ReadLine();
                _service.Cancel(code);
                _displayer.ShowPartialBill();
            }
            catch (Exception e)
                when (e is InvalidBarCodeException || e is BoughtProductNotFoundException)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SetUpPriceLimit()
        {
            var limit = ReadDecimalFromKeyboard("Please enter price limit (0 for no limit): ");
            _service.SetUpLimit(limit);
        }

        private static decimal ReadDecimalFromKeyboard(string initialText)
        {
            Console.Write(initialText);
            string numberAsText = Console.ReadLine();
            decimal number;
            while (!decimal.TryParse(numberAsText, out number))
            {
                Console.Write("The text you entered isn't a valid number. Please try again: ");
                numberAsText = Console.ReadLine();
            }

            return number;
        }
    }
}
