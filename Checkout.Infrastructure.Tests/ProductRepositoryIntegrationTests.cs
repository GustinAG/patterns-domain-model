using System;
using Checkout.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout.Infrastructure.Tests
{
    [TestClass]
    public class ProductRepositoryIntegrationTests
    {
        [TestMethod]
        public void FindBy_ShouldFindProduct()
        {
            // Arrange
            var repository = new ProductRepository();

            // Act
            var product = repository.FindBy(new BarCode("555"));

            // Assert
            Console.WriteLine(product);
            product.Should().NotBeNull();
            product.Should().NotBeEquivalentTo(Product.NoProduct);
        }

        [TestMethod]
        public void FindBy_ShouldFindNoProduct_WhenBarCodeNotAssociated()
        {
            // Arrange
            var repository = new ProductRepository();

            // Act
            var product = repository.FindBy(new BarCode("000"));

            // Assert
            Console.WriteLine(product);
            product.Should().NotBeNull();
            product.Should().BeEquivalentTo(Product.NoProduct);
        }

    }
}
