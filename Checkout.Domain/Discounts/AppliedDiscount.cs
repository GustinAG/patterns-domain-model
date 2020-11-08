using System.Collections.Generic;

namespace Checkout.Domain.Discounts
{
    public class AppliedDiscount : ValueObject
    {
        internal AppliedDiscount(string name, decimal subTotal)
        {
            Name = name;
            SubTotal = subTotal;
        }

        public string Name { get; }
        public decimal SubTotal { get; }

        protected override IList<object> EqualityComponents => new List<object> { Name, SubTotal };
    }
}
