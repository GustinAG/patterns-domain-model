using Dawn;
using DomainModel.Domain.Discounts;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
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

        /// <summary>
        /// Starts the checkout process.
        /// </summary>
        public void Start()
        {
            Guard.Operation(_state == ProcessState.NotStartedYet, $"You cannot start a checkout process when {_state}");
            _bill = Bill.EmptyBill;
            _state = ProcessState.InProgress;
        }

        /// <summary>
        /// Scans the bar code of a product.
        /// </summary>
        public void Scan(BarCode barCode)
        {
            Guard.Operation(_state == ProcessState.InProgress, $"You mustn't scan a bought product when checkout process {_state}");
            var product = FindProductBy(barCode);
            if (product == Product.NoProduct) throw new InvalidBarCodeException(barCode);

            _bill = _bill.AddOne(product);

            CheckIfLimitExceeded();
        }

        /// <summary>
        /// Cancels an already scanned product.
        /// </summary>
        /// <remarks>HU: "sztornóz".</remarks>
        public void Cancel(BarCode barCode)
        {
            Guard.Operation(_state == ProcessState.InProgress, $"You can only cancel items when checkout process {ProcessState.InProgress}");
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
            if (_limit.IsExceededBy(_bill.TotalPrice)) CheckoutLimitExceeded?.Invoke(_limit, _bill.TotalPrice);
        }

        public Bill ShowBill() => _bill;
        public delegate void CheckoutLimitExceededDelegate(CheckoutLimit limit, decimal currentPrice);
        public event CheckoutLimitExceededDelegate CheckoutLimitExceeded;

        public void Close()
        {
            Guard.Operation(_state == ProcessState.InProgress, $"You cannot close the checkout process when {_state}");
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
