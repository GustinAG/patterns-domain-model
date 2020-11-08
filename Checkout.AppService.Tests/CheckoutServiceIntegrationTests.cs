using System;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using Checkout.Repositories;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Checkout.AppService.Tests
{
    [TestClass]
    public class CheckoutServiceIntegrationTests
    {
        private const string TestProductName = "Test Product";
        private const string ValidCode = "1234";
        private const string InvalidCode = "000";

        private static readonly Action<decimal, decimal> EmptyAction = (a, b) => { };
        private static readonly Product TestProduct = new Product(TestProductName, 0.98M);

        [TestMethod]
        public void Start_ShouldProduceEmptyBill()
        {
            // Arrange
            var service = CreateService();

            // Act
            service.Start(EmptyAction);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().Be(Bill.EmptyBill);
        }

        [TestMethod]
        public void Scan_ShouldProduceNonEmptyBill()
        {
            // Arrange
            var service = CreateService();
            service.Start(EmptyAction);

            // Act
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().NotBeEquivalentTo(Bill.EmptyBill);
        }

        [TestMethod]
        public void Scan_ShouldProduceBillWithScannedProduct()
        {
            // Arrange
            var service = CreateService();
            service.Start();

            // Act
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.GroupedBoughtProducts.Should().Contain(p => p.Key == TestProduct);
        }

        [TestMethod]
        public void Scan_ShouldProduceTOneGroup_WhenSameProductScanned()
        {
            // Arrange
            var service = CreateService();
            service.Start(EmptyAction);

            // Act
            service.Scan(ValidCode);
            service.Scan(ValidCode);
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.GroupedBoughtProducts.Should().HaveCount(1);
        }

        [TestMethod]
        public void Scan_ShouldThrowExceptionForInvalidBarCode()
        {
            // Arrange
            var service = CreateService(false);
            Action scanAction = () => service.Scan(InvalidCode);
            service.Start(EmptyAction);

            // Act & Assert
            scanAction.Should().Throw<InvalidBarCodeException>();
        }

        [TestMethod]
        public void Scan_ShouldThrowException_WhenCheckoutProcessClosed()
        {
            // Arrange
            var service = CreateService();
            Action scanAction = () => service.Scan(ValidCode);
            service.Start(EmptyAction);
            service.Close();

            // Act & Assert
            scanAction.Should().Throw<InvalidOperationException>();
        }

        private static CheckoutService CreateService(bool mockRepository = true)
        {
            var repository = mockRepository ? CreateMockedRepository() : new ProductRepository();
            return new CheckoutService(new OutChecker(repository));
        }

        private static IProductRepository CreateMockedRepository()
        {
            var repository = Substitute.For<IProductRepository>();

            repository.FindBy(new BarCode(ValidCode)).Returns(TestProduct);
            repository.FindBy(Arg.Any<string>()).Returns(Product.NoProduct);

            return repository;
        }
    }
}
