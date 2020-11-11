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
                var productItems = _bill.GroupedBoughtProducts.Select(ProductLineItem);
                var discountItems = _bill.AppliedDiscounts.Select(d => new BillLineItem(null, d.Name, d.SubTotal));
                var detailedItems = productItems.Concat(discountItems).OrderBy(l => l.Description);
                var allLines = detailedItems.Select(l => l.AsThreeColumnLine).Append(DashedLine).Append(SummaryLine);

                return string.Join(Environment.NewLine, allLines);
            }
        }

        private static BillLineItem ProductLineItem(KeyValuePair<Product, int> productLine)
        {
            var (product, count) = productLine;
            var unitPrice = product.Price;
            return new BillLineItem(count, Invariant($"{product.Name} (unit price: {unitPrice.AsPriceText()})"), count * unitPrice);
        }

        private string SummaryLine => new BillLineItem(null, "TOTAL      ", _bill.TotalPrice).AsThreeColumnLine;

        private static string LastAddedLine(Product product) =>
            product == Product.NoProduct
                ? " -"
                : new BillLineItem(null, product.Name, product.Price).AsThreeColumnLine;
    }
}
