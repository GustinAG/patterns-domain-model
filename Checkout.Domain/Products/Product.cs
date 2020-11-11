using System.Collections.Generic;

namespace Checkout.Domain.Products
{
    public sealed class Product : AggregateRoot
    {
        public Product(string name, decimal price, bool isAdult = false)
        {
            Name = name;
            Price = price;
            IsAdult = isAdult;
        }

        public static Product NoProduct { get; } = new Product(string.Empty, decimal.MinusOne);

        public string Name { get; }
        public decimal Price { get; }
        public bool IsAdult { get; }

        protected override IList<object> EqualityComponents => new List<object> { Name, Price, IsAdult };
    }
}
