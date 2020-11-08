using System;
using System.Collections.Generic;
using System.Linq;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;

namespace Checkout.Domain.Discounts
{
    internal class VolumeDiscountStrategy : IDiscountStrategy
    {
        private const int VolumeThreshold = 4;
        private const decimal VolumeDiscountRate = 0.1M;

        public IReadOnlyList<AppliedDiscount> Calculate(BoughtProducts boughtProducts)
        {
            var productGroups = boughtProducts.GroupedByProduct;
            return productGroups.Where(g => g.Value >= VolumeThreshold).Select(CreateAppliedDiscount).ToList();
        }

        private static AppliedDiscount CreateAppliedDiscount(KeyValuePair<Product, int> productGroup)
        {
            var (product, count) = productGroup;
            return new AppliedDiscount(VolumeDiscountName(product), VolumeDiscountSubTotal(product, count));
        }

        private static string VolumeDiscountName(Product product) => $"{product.Name} volume discount";
        private static decimal VolumeDiscountSubTotal(Product product, int count) => -Math.Round(product.Price * count * VolumeDiscountRate, 2);
    }
}
