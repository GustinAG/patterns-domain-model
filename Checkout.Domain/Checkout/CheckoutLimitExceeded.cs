namespace Checkout.Domain.Checkout
{
    public sealed class CheckoutLimitExceeded : DomainEvent
    {
        public CheckoutLimit Limit { get; }
        public decimal Price { get; }

        internal CheckoutLimitExceeded(CheckoutLimit limit, decimal price)
        {
            Limit = limit;
            Price = price;
        }
    }
}
