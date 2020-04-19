using DomainModel.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DomainModel.AppService.Tests
{
    [TestClass]
    public class RegisteringResolverUnitTests
    {
        [TestMethod]
        public void Resolve_ShouldGiveDefaultValue_WhenNothingRegistered()
        {
            // Arrange
            var resolver = new RegisteringResolver();

            // Act
            var instance = resolver.Resolve<IProductRepository>();

            // Assert
            instance.Should().BeNull("nothing registered");
        }

        [TestMethod]
        public void Resolve_ShouldGiveRegisteredType()
        {
            // Arrange
            var resolver = new RegisteringResolver();
            resolver.Register<IProductRepository>(() => Substitute.For<IProductRepository>());

            // Act
            var instance = resolver.Resolve<IProductRepository>();

            // Assert
            instance.Should().NotBeNull();
            instance.Should().BeAssignableTo<IProductRepository>();
        }

        [TestMethod]
        public void Resolve_ShouldGiveRegisteredInstance()
        {
            // Arrange
            var resolver = new RegisteringResolver();
            var repository = Substitute.For<IProductRepository>();
            resolver.Register<IProductRepository>(() => repository);

            // Act
            var instance = resolver.Resolve<IProductRepository>();

            // Assert
            instance.Should().Be(repository);
        }
    }
}
