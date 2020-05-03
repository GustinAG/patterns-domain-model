using Dawn;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Limiter;
using DomainModel.Domain.Products;

namespace DomainModel.AppService
{
    public class CheckoutService
    {
        private readonly IResolver _resolver;
        private OutChecker _outChecker;
        public CreditLimit CreditLimit { get; set; }

        public CheckoutService(IResolver resolver = null)
        {
            _resolver = resolver ?? DefaultResolver.Create();
        }

        public void Start()
        {
            var repository = _resolver.Resolve<IProductRepository>();
            _outChecker = new OutChecker(repository);
            _outChecker.Start();
        }

        public string Scan(string code)
        {
            Guard.Operation(_outChecker != null);
            var barCode = new BarCode(code);
            return _outChecker.Scan(barCode);
        }

        public string Storno(string code)
        {
            Guard.Operation(_outChecker != null);
            var barCode = new BarCode(code);
            return _outChecker.Storno(barCode);
        }

        public void Close()
        {
            Guard.Operation(_outChecker != null);
            _outChecker.Close();
        }

        public string GetCurrentBill()
        {
            Guard.Operation(_outChecker != null);
            return _outChecker.ShowBill().PrintableText;
        }

        public decimal GetCurrentBillValue()
        {
            Guard.Operation(_outChecker != null);
            return _outChecker.ShowBill().SumPriceOfBoughtProducts;
        }
    }
}
