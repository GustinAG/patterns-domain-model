using System.Collections.Generic;
using System.Linq;
using DomainModel.Domain.Products;

namespace DomainModel.Domain.Checkout
{
    internal sealed class BoughtProducts : ValueObject
    {
        private readonly IReadOnlyList<Product> _products;

        /// <summary>
        /// Creates a new, empty collection fo bought products.
        /// </summary>
        private BoughtProducts(IReadOnlyList<Product> products = null)
        {
            _products = products ?? new List<Product>();
        }

        internal static BoughtProducts Undefined { get; } = new BoughtProducts(new List<Product> { Product.NoProduct });
        internal static BoughtProducts NoProducts { get; } = new BoughtProducts();

        internal decimal TotalPrice => _products.Sum(p => p.Price);
        internal IReadOnlyDictionary<Product, int> GroupedByProduct => _products.GroupBy(p => p).ToDictionary(p => p.Key, p => p.Count());

        internal int CountOf(Product product)
        {
            var groups = GroupedByProduct;
            return groups.ContainsKey(product) ? groups[product] : 0;
        }

        internal BoughtProducts Add(Product product)
        {
            var products = _products.ToList();
            products.Add(product);
            return new BoughtProducts(products);
        }

        internal BoughtProducts RemoveLast(Product product)
        {
            var products = _products.ToList();
            int lastIndex = products.LastIndexOf(product);
            if (lastIndex < 0)
            {
                throw new BoughtProductNotFoundException(product);
            }
            products.RemoveAt(lastIndex);
            return new BoughtProducts(products);
        }

        internal Product LastScannedProduct => _products.Last();

        protected override IList<object> EqualityComponents => new List<object>(_products);
    }
}
