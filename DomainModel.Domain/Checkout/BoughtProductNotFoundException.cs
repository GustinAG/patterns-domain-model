using System;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
{
    public class BoughtProductNotFoundException : Exception
    {
        public BoughtProductNotFoundException(Product product) : base($"Invalid product '{product?.Name}' - product not bought yet!")
        { }
    }
}
