using System;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
{
    public class EmptyBillStornoCodeException : Exception
    {
        public EmptyBillStornoCodeException() : base($"There is no product, you can not storno anything!")
        { }
    }
}
