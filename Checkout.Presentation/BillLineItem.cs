using static System.FormattableString;

namespace Checkout.Presentation
{
    internal class BillLineItem
    {
        private readonly int? _count;
        private readonly decimal _price;

        internal BillLineItem(int? count, string description, decimal price)
        {
            Description = description;
            _count = count;
            _price = price;
        }

        internal string Description { get; }

        internal string AsThreeColumnLine => Invariant($"{_count,2} {Description,40} {_price.AsPriceText(),8}");
    }
}
