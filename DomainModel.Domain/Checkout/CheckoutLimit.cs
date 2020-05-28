using System.Collections.Generic;
using DomainModel.Domain.Checkout;

namespace DomainModel.Domain.Products
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

        protected override IList<object> EqualityComponents => new List<object> { Limit };
    }
}
