using DomainModel.Domain.Products;

namespace DomainModel.Repositories
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

        public Product FindBy(BarCode barCode)
        {
            int.TryParse(barCode.Code, out var code);
            code %= Products.Length;

            return Products[code];
        }
    }
}
