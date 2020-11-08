using System.Collections.Generic;

namespace Checkout.Domain.Checkout
{
    public class CheckoutLimit : ValueObject
    {
        public static CheckoutLimit NoLimit { get; } = new CheckoutLimit(0);

        public CheckoutLimit(decimal limit)
        {
            if (limit < 0) throw new InvalidCheckoutLimitException(limit);
            Limit = limit;
        }

        public decimal Limit { get; }

        internal bool IsExceededBy(decimal price) => this != NoLimit && Limit < price;

        protected override IList<object> EqualityComponents => new List<object> { Limit };
    }
}
