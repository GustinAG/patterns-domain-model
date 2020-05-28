using System;
using Dawn;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Products;
using static DomainModel.Domain.Checkout.OutChecker;

namespace DomainModel.AppService
{
    public class CheckoutService
    {
        private readonly IResolver _resolver;
        private OutChecker _outChecker;

        public CheckoutService(IResolver resolver = null)
        {
            _resolver = resolver ?? DefaultResolver.Create();
        }

        public void Start(Action<decimal, decimal> limitExceededAction)
        {
            var repository = _resolver.Resolve<IProductRepository>();
            _outChecker = new OutChecker(repository);

            CheckoutLimitExceededDelegate checkoutLimitExceeded = (limit, currentPrice) => {
                limitExceededAction(limit.Limit, currentPrice);
            };

            _outChecker.CheckoutLimitExceeded += checkoutLimitExceeded;
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
