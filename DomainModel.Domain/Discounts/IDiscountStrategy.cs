using System.Collections.Generic;
using DomainModel.Domain.Checkout;

namespace DomainModel.Domain.Discounts
{
    internal interface IDiscountStrategy
    {
        IReadOnlyList<AppliedDiscount> Calculate(BoughtProducts boughtProducts);
    }
}
