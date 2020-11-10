using System.Linq;
using Checkout.Domain.Discounts;
using Checkout.Domain.Products;
using Dawn;

namespace Checkout.Domain.Checkout
{
    /// <summary>
    /// Domain service doing the checkout process.
    /// </summary>
    /// <remarks>
    /// Hopefully, my English isn't so bad...
    /// See also: https://www.grammarphobia.com/blog/2019/05/checkout.html
    /// </remarks>
    public sealed class OutChecker
    {
        private readonly IProductRepository _repository;
        private ProcessState _state = ProcessState.NotStartedYet;
        private Bill _bill = Bill.NoBill;
        private CheckoutLimit _limit = CheckoutLimit.NoLimit;

        public OutChecker(IProductRepository repository)
        {
            _repository = repository;
        }

        public bool CanStart => _state == ProcessState.NotStartedYet || _state == ProcessState.Closed;

        /// <summary>
        /// Starts the checkout process.
        /// </summary>
        public void Start()
        {
            Guard.Operation(CanStart, $"You cannot start a checkout process when {_state}");
            _bill = Bill.EmptyBill;
            _state = ProcessState.InProgress;
        }

        public bool CanScan => _state == ProcessState.InProgress;

        /// <summary>
        /// Scans the bar code of a product.
        /// </summary>
        public void Scan(BarCode barCode)
        {
            Guard.Operation(CanScan, $"You mustn't scan a bought product when checkout process {_state}");
            var product = FindProductBy(barCode);
            if (product == Product.NoProduct) throw new InvalidBarCodeException(barCode);

            _bill = _bill.AddOne(product);

            CheckIfLimitExceeded();
        }

        public bool CanCancel => _state == ProcessState.InProgress && _bill.GroupedBoughtProducts.Any();

        /// <summary>
        /// Cancels an already scanned product.
        /// </summary>
        /// <remarks>HU: "sztornóz".</remarks>
        public void Cancel(BarCode barCode)
        {
            Guard.Operation(CanCancel, $"You can only cancel items when checkout process {ProcessState.InProgress} and any product already scanned");
            var product = FindProductBy(barCode);
            if (product == Product.NoProduct) throw new InvalidBarCodeException(barCode);

            _bill = _bill.CancelOne(product);
        }

        public void SetUpLimit(CheckoutLimit limit)
        {
            Guard.Operation(_state == ProcessState.InProgress, $"You can only cancel items when checkout process {ProcessState.InProgress}");

            _limit = limit;
            CheckIfLimitExceeded();
        }

        private void CheckIfLimitExceeded()
        {
            // TODO: introduce domain event instead!
            if (_limit.IsExceededBy(_bill.NoDiscountTotalPrice)) CheckoutLimitExceeded?.Invoke(_limit, _bill.NoDiscountTotalPrice);
        }

        public Bill ShowBill() => _bill;
        public delegate void CheckoutLimitExceededDelegate(CheckoutLimit limit, decimal currentPrice);
        public event CheckoutLimitExceededDelegate CheckoutLimitExceeded;

        public bool CanClose => _state == ProcessState.InProgress;

        public void Close()
        {
            Guard.Operation(CanClose, $"You cannot close the checkout process when {_state}");
            _bill = _bill.ApplyDiscounts(new Discounter(_repository));
            _state = ProcessState.Closed;
        }

        private Product FindProductBy(BarCode barCode)
        {
            var product = _repository.FindBy(barCode);
            Guard.Operation(product != null, "Repository error: null reference received");
            return product;
        }
    }
}
