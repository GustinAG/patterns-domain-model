using static System.FormattableString;

namespace Checkout.Presentation
{
    internal static class Extensions
    {
        internal static string AsPriceText(this decimal price) => Invariant($"€ {price:f2}");
    }
}
