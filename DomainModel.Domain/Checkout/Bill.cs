using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Domain.Discounts;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
{
    public class Bill : AggregateRoot
    {
        private const string NoBillText = "No bill available!";
        private const string EmptyBillText = "Empty bill - nothing bought.";
        private static readonly string DashedLine = new string('-', 60);

        private readonly BoughtProducts _boughtProducts;
        private readonly IReadOnlyList<AppliedDiscount> _appliedDiscounts;

        private Bill(BoughtProducts products, IReadOnlyList<AppliedDiscount> appliedDiscounts)
        {
            _boughtProducts = products;
            _appliedDiscounts = appliedDiscounts;
        }

        public static Bill NoBill { get; } = new Bill(BoughtProducts.Undefined, Discounter.NoAppliedDiscounts);
        public static Bill EmptyBill { get; } = new Bill(BoughtProducts.NoProducts, Discounter.NoAppliedDiscounts);

        public string PrintableText
        {
            get
            {
                if (_boughtProducts == BoughtProducts.Undefined) return NoBillText;
                if (_boughtProducts == BoughtProducts.NoProducts) return EmptyBillText;

                var productLines = _boughtProducts.GroupedByProduct.Select(PrintableProductLine);
                var discountLines = _appliedDiscounts.Select(d => ThreeColumnLine(string.Empty, d.Name, d.SubTotal));
                var allLines = productLines.Union(discountLines).Append(DashedLine).Append(SummaryLine);

                return string.Join(Environment.NewLine, allLines);
            }
        }

        public string PrintableLastAddedProductText
        {
            get
            {
                if (_boughtProducts == BoughtProducts.Undefined) return NoBillText;
                if (_boughtProducts == BoughtProducts.NoProducts) return EmptyBillText;

                return LastAddedLine(_boughtProducts.LastAddedProduct);
            }
        }

        internal Bill Add(Product product) => new Bill(_boughtProducts.Add(product), _appliedDiscounts);
        
        internal Bill CancelOne(Product product)
        {
            var bill = new Bill(_boughtProducts.RemoveOne(product), _appliedDiscounts);
            return bill;
        }

        internal Bill ApplyDiscounts(Discounter discounter)
        {
            var discounts = discounter.CalculateDiscounts(_boughtProducts);
            return new Bill(_boughtProducts, discounts.ToList());
        }

        protected override IList<object> EqualityComponents => new List<object> { _boughtProducts };

        private string SummaryLine => ThreeColumnLine(string.Empty, "TOTAL      ", $"€ {_boughtProducts.TotalPrice:f2}");
        private string LastAddedLine(Product product) => ThreeColumnLine(product.Name, $"€ {product.Price:f2}", $"€ {_boughtProducts.TotalPrice:f2}");

        private static string PrintableProductLine(KeyValuePair<Product, int> productLine)
        {
            var count = productLine.Value;
            var product = productLine.Key;
            var unitPrice = product.Price;
            return ThreeColumnLine(count, $"{product.Name} (unit price: € {unitPrice:f2})", $"€ {count * unitPrice:f2}");
        }

        private static string ThreeColumnLine(object a, object b, object c) => $"{a,2} {b,40} {c,8}";
    }
}
