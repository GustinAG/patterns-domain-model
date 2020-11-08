using System.Collections.Generic;
using Checkout.Domain.Checkout;

namespace Checkout.Domain.Discounts
{
    internal interface IDiscountStrategy
    {
        IReadOnlyList<AppliedDiscount> Calculate(BoughtProducts boughtProducts);
    }
}
