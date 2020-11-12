using System;
using Checkout.Domain.Products;
using static System.FormattableString;

namespace Checkout.Domain.Checkout
{
    public class AdultProductBuyingNotAllowedException : Exception
    {
        public AdultProductBuyingNotAllowedException(Customer customer, Product product) :
            base(Invariant($"{customer} customer buying adult product {product} - not allowed!"))
        { }
    }
}
