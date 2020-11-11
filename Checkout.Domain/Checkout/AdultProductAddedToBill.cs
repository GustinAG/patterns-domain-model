using Checkout.Domain.Products;

namespace Checkout.Domain.Checkout
{
    public sealed class AdultProductAddedToBill : DomainEvent
    {
        internal AdultProductAddedToBill(Product product)
        {
            Product = product;
        }

        public Product Product { get; }
    }
}
