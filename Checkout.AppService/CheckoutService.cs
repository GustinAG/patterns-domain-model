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
            Guard.Operation(outChecker != null);
            _outChecker = outChecker;
        }

        public void Start(Action<decimal, decimal> limitExceededAction = null)
        {
            _outChecker.CheckoutLimitExceeded += (l, p) => limitExceededAction?.Invoke(l.Limit, p);
            _outChecker.Start();
        }

        public void Scan(string code)
        {
            var barCode = new BarCode(code);
            _outChecker.Scan(barCode);
        }

        public void Cancel(string code)
        {
            var barCode = new BarCode(code);
            _outChecker.Cancel(barCode);
        }

        public void Close() => _outChecker.Close();

        public void SetUpLimit(decimal limit) => _outChecker.SetUpLimit(new CheckoutLimit(limit));

        public Bill GetCurrentBill() => _outChecker.ShowBill();

        public bool CanStart => _outChecker.CanStart;
        public bool CanScan => _outChecker.CanScan;
        public bool CanCancel => _outChecker.CanCancel;
        public bool CanClose => _outChecker.CanClose;
    }
}
