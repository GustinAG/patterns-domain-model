using Checkout.Contracts;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using Checkout.Infrastructure;
using Dawn;
using static System.FormattableString;

namespace Checkout.AppService
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IDomainEventRegistry _eventRegistry;
        private readonly OutChecker _outChecker;
        private readonly IWarningPresenter _presenter;

        public CheckoutService(IDomainEventRegistry eventRegistry, OutChecker outChecker, IWarningPresenter presenter)
        {
            Guard.Operation(outChecker != null);
            _eventRegistry = eventRegistry;
            _outChecker = outChecker;
            _presenter = presenter;
        }

        public void Start()
        {
            _eventRegistry.Register<CheckoutLimitExceeded>(WarnLimitExceeded);
            _eventRegistry.Register<AdultProductAddedToBill>(WarnAdultProduct);
            _outChecker.Start();
        }

        public void Scan(string code)
        {
            var barCode = new BarCode(code);
            _outChecker.Scan(barCode);
            _eventRegistry.PlayAll();
        }

        public void Cancel(string code)
        {
            var barCode = new BarCode(code);
            _outChecker.Cancel(barCode);
        }

        public void Close() => _outChecker.Close();

        public void SetUpLimit(decimal limit)
        {
            _outChecker.SetUpLimit(new CheckoutLimit(limit));
            _eventRegistry.PlayAll();
        }

        public Bill GetCurrentBill() => _outChecker.ShowBill();

        public bool CanStart => _outChecker.CanStart;
        public bool CanScan => _outChecker.CanScan;
        public bool CanCancel => _outChecker.CanCancel;
        public bool CanSetUpLimit => _outChecker.CanSetUpLimit;
        public bool CanClose => _outChecker.CanClose;

        private void WarnLimitExceeded(CheckoutLimitExceeded e) =>
            _presenter.ShowWarning(Invariant($"Warning: Your limit has been exceeded (limit: € {e.Limit}, current price: € {e.Price})"));

        private void WarnAdultProduct(AdultProductAddedToBill e) => _presenter.ShowWarning(Invariant($"Warning: ADULT PRODUCT {e.Product.Name} added to bill !!!"));
    }
}
