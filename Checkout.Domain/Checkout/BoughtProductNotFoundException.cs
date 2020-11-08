using System;
using Checkout.Domain.Products;

namespace Checkout.Domain.Checkout
{
    public class BoughtProductNotFoundException : Exception
    {
        public BoughtProductNotFoundException(Product product) : base($"Invalid product '{product?.Name}' - product not bought yet!")
        { }
    }
}
