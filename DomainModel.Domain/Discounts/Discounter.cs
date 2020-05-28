using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Discounts
{
    internal class Discounter
    {
        private readonly IReadOnlyList<IDiscountStrategy> _discounts;

        internal static IReadOnlyList<AppliedDiscount> NoAppliedDiscounts { get; } = Array.Empty<AppliedDiscount>();

        internal Discounter(IProductRepository repository)
        {
            _discounts = new IDiscountStrategy[] { new VolumeDiscountStrategy(), new CombinedSaleDiscountStrategy(repository) };
        }

        internal IReadOnlyList<AppliedDiscount> CalculateDiscounts(BoughtProducts boughtProducts) =>
            _discounts.SelectMany(d => d.Calculate(boughtProducts)).ToList();
    }
}
