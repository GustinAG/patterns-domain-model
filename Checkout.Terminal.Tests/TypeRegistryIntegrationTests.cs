using Autofac;
using Checkout.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout.Terminal.Tests
{
    [TestClass]
    public class TypeRegistryIntegrationTests
    {
        [TestMethod]
        public void Build_ShouldBuildContainer()
        {
            // Act
            var container = TypeRegistry.Build();

            // Assert
            container.Should().NotBeNull();
        }

        [TestMethod]
        public void Build_ShouldResolveMainProcess()
        {
            // Act
            var container = TypeRegistry.Build();

            // Assert
            container.Should().NotBeNull();
            using var scope = container.BeginLifetimeScope();
            var process = scope.Resolve<MainProcess>();
            process.Should().NotBeNull();
        }

        [TestMethod]
        public void Build_ShouldResolveRepository()
        {
            // Act
            var container = TypeRegistry.Build();

            // Assert
            container.Should().NotBeNull();
            using var scope = container.BeginLifetimeScope();
            var repository = scope.Resolve<IProductRepository>();
            repository.Should().NotBeNull();
        }
    }
}
