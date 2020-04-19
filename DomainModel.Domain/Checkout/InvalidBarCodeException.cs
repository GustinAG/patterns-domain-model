using System;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
{
    public class InvalidBarCodeException : Exception
    {
        public InvalidBarCodeException(BarCode barCode) : base($"Invalid bar code '{barCode?.Code}' - no such product!")
        { }
    }
}
