using System;
using Checkout.Domain.Products;

namespace Checkout.Domain.Checkout
{
    public class InvalidBarCodeException : Exception
    {
        public InvalidBarCodeException(BarCode barCode) : base($"Invalid bar code '{barCode?.Code}' - no such product!")
        { }
    }
}
