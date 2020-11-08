using System;
using System.Collections.Generic;
using System.Linq;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using static System.FormattableString;

namespace Checkout.Presentation
{
    public sealed class BillAppearance
    {
        private const string NoBillText = "No bill available!";
        private const string EmptyBillText = "Empty bill - nothing bought.";
        private static readonly string DashedLine = new string('-', 60);

        private readonly Bill _bill;

        public BillAppearance(Bill bill)
        {
            _bill = bill;
        }

        public string AsText =>
            _bill switch
            {
                { } bill when bill == Bill.NoBill => NoBillText,
                { } bill when bill == Bill.EmptyBill => EmptyBillText,
                _ => DetailedBillText
            };

        public string LastAddedProductAsText => LastAddedLine(_bill.LastAddedProduct);

        private string DetailedBillText
        {
            get
            {
                var productLines = _bill.GroupedBoughtProducts.Select(PrintableProductLine);
                var discountLines = _bill.AppliedDiscounts.Select(d => ThreeColumnLine(string.Empty, d.Name, d.SubTotal));
                var allLines = productLines.Union(discountLines).Append(DashedLine).Append(SummaryLine);

                return string.Join(Environment.NewLine, allLines);
            }
        }

        private static string PrintableProductLine(KeyValuePair<Product, int> productLine)
        {
            var (product, count) = productLine;
            var unitPrice = product.Price;
            return ThreeColumnLine(count, Invariant($"{product.Name} (unit price: € {unitPrice:f2})"), Invariant($"€ {count * unitPrice:f2}"));
        }

        private string SummaryLine => ThreeColumnLine(string.Empty, "TOTAL      ", Invariant($"€ {_bill.TotalPrice:f2}"));

        private static string LastAddedLine(Product product) =>
            product == Product.NoProduct
                ? " -"
                : ThreeColumnLine(product.Name, Invariant($"€ {product.Price:f2}"), string.Empty);

        private static string ThreeColumnLine(object a, object b, object c) => $"{a,2} {b,40} {c,8}";
    }
}
