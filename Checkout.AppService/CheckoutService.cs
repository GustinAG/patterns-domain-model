using System;
using Checkout.Contracts;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using Dawn;

namespace Checkout.AppService
{
    public class CheckoutService : ICheckoutService
    {
        private readonly OutChecker _outChecker;

        public CheckoutService(OutChecker outChecker)
        {
            _outChecker = outChecker;
        }

        public void Start(Action<decimal, decimal> limitExceededAction)
        {
            _outChecker.CheckoutLimitExceeded += (l, p) => limitExceededAction(l.Limit, p);
            _outChecker.Start();
        }

        public void Scan(string code)
        {
            Guard.Operation(_outChecker != null);
            var barCode = new BarCode(code);
            _outChecker.Scan(barCode);
        }

        public void Cancel(string code)
        {
            Guard.Operation(_outChecker != null);
            var barCode = new BarCode(code);
            _outChecker.Cancel(barCode);
        }

        public void Close()
        {
            Guard.Operation(_outChecker != null);
            _outChecker.Close();
        }

        public void SetUpLimit(decimal limit)
        {
            Guard.Operation(_outChecker != null);
            _outChecker.SetUpLimit(new CheckoutLimit(limit));
        }

        public string GetCurrentBill()
        {
            Guard.Operation(_outChecker != null);
            return _outChecker.ShowBill().PrintableText;
        }

        public string GetLastAdded()
        {
            Guard.Operation(_outChecker != null);
            return _outChecker.ShowBill().PrintableLastAddedProductText;
        }
    }
}
