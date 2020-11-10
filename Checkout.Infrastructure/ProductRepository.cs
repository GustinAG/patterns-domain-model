using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Checkout.Domain.Products;
using Dawn;

namespace Checkout.Infrastructure
{
    // TODO: Real implementation with MongoDB  e.g.!
    /// <summary>
    /// Fake repository implementation - for demo purposes only.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private static readonly Product[] Products =
        {
            Product.NoProduct,
            new Product ("Egg", 0.11M),
            new Product ("Bread", 0.89M),
            new Product ("Butter", 0.39M),
            new Product ("Grape", 1.00M),
            new Product ("Orange", 1.12M),
            new Product ("Milk", 0.65M),
            new Product ("Sugar", 0.43M),
            new Product ("Cucumber", 0.25M),
            new Product ("Crescent", 0.2M),
            new Product ("Handkerchief", 0.98M),
            new Product ("Pencil", 0.5M)
        };

        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Guard validates it here.")]
        public Product FindBy(BarCode barCode)
        {
            Guard.Argument(barCode, nameof(barCode)).NotNull();
            if (!int.TryParse(barCode.Code, out var code)) return Product.NoProduct;

            code %= Products.Length;

            return Products[code];
        }

        public Product FindBy(string name) => Products.FirstOrDefault(p => p.Name == name) ?? Product.NoProduct;
    }
}
