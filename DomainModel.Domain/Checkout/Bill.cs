using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
{
    public class Bill : AggregateRoot
    {
        private const string NoBillText = "No bill available!";
        private const string EmptyBillText = "Empty bill - nothing bought.";
        private static readonly string DashedLine = new string('-', 60);

        private readonly BoughtProducts _boughtProducts;

        private Bill(BoughtProducts products)
        {
            _boughtProducts = products;
        }

        public static Bill NoBill { get; } = new Bill(BoughtProducts.Undefined);
        public static Bill EmptyBill { get; } = new Bill(BoughtProducts.NoProducts);

        public string PrintableText
        {
            get
            {
                if (_boughtProducts == BoughtProducts.Undefined) return NoBillText;
                if (_boughtProducts == BoughtProducts.NoProducts) return EmptyBillText;

                return string.Join(Environment.NewLine, _boughtProducts.GroupedByProduct.Select(PrintableProductLine).Append(DashedLine).Append(SummaryLine));
            }
        }

        public Bill Add(Product product) => new Bill(_boughtProducts.Add(product));

        protected override IList<object> EqualityComponents => new List<object> { _boughtProducts };

        private string SummaryLine => ThreeColumnLine(string.Empty, "TOTAL      ", $"€ {_boughtProducts.TotalPrice:f2}");

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
