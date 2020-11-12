using System.Collections.Generic;
using System.Linq;
using Checkout.Domain.Discounts;
using Checkout.Domain.Products;

namespace Checkout.Domain.Checkout
{
    public class Bill : AggregateRoot
    {
        private readonly BoughtProducts _boughtProducts;

        private Bill(BoughtProducts products, IReadOnlyList<AppliedDiscount> appliedDiscounts)
        {
            _boughtProducts = products;
            AppliedDiscounts = appliedDiscounts;
        }

        public static Bill NoBill { get; } = new Bill(BoughtProducts.Undefined, Discounter.NoAppliedDiscounts);
        public static Bill EmptyBill { get; } = new Bill(BoughtProducts.NoProducts, Discounter.NoAppliedDiscounts);

        public decimal NoDiscountTotalPrice => _boughtProducts.TotalPrice;
        public decimal TotalDiscount => AppliedDiscounts.Sum(d => d.SubTotal);
        public decimal TotalPrice => NoDiscountTotalPrice + TotalDiscount;

        public Product LastAddedProduct => _boughtProducts.LastAddedProduct;

        public IReadOnlyDictionary<Product, int> GroupedBoughtProducts => _boughtProducts.GroupedByProduct;

        public IReadOnlyList<AppliedDiscount> AppliedDiscounts { get; }

        internal Bill AddOne(Product product) => new Bill(_boughtProducts.AddOne(product), AppliedDiscounts);

        internal Bill CancelOne(Product product) => new Bill(_boughtProducts.RemoveOne(product), AppliedDiscounts);

        internal Bill ApplyDiscounts(Discounter discounter)
        {
            var discounts = discounter.CalculateDiscounts(_boughtProducts);
            return new Bill(_boughtProducts, discounts.ToList());
        }

        protected override IList<object> EqualityComponents => new List<object> { _boughtProducts, AppliedDiscounts };
    }
}
