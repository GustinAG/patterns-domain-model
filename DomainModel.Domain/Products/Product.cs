using System.Collections.Generic;

namespace DomainModel.Domain.Products
{
    public sealed class Product : AggregateRoot
    {
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public static Product NoProduct { get; } = new Product(string.Empty, decimal.MinusOne);

        public string Name { get; }
        public decimal Price { get; }

        protected override IList<object> EqualityComponents => new List<object> { Name, Price };
    }
}
