using System;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
{
    public class NoProductBillStornoCodeException : Exception
    {
        public NoProductBillStornoCodeException(BarCode barCode) : base($"Invalid bar code '{barCode?.Code}' - no such product in the bill!")
        { }
    }
}
