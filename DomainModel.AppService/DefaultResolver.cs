using DomainModel.Domain.Products;
using DomainModel.Repositories;

namespace DomainModel.AppService
{
    internal sealed class DefaultResolver : RegisteringResolver
    {
        private DefaultResolver()
        { }

        internal static DefaultResolver Create()
        {
            var resolver = new DefaultResolver();
            resolver.Register<IProductRepository>(() => new ProductRepository());
            return resolver;
        }
    }
}
