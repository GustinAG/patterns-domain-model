using System;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DomainModel.AppService.Tests
{
    [TestClass]
    public class CheckoutServiceIntegrationTests
    {
        private const string TestProductName = "Test Product";
        private const string ValidCode = "1234";

        [TestMethod]
        public void Start_ShouldProduceEmptyBill()
        {
            // Arrange
            var service = GetDefaultService();

            // Act
            service.Start();

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().Be(Bill.EmptyBill.PrintableText);
        }

        [TestMethod]
        public void Scan_ShouldProduceBillWithScannedProduct()
        {
            // Arrange
            var service = GetDefaultService();
            service.Start();

            // Act
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().Contain(TestProductName);
        }

        [TestMethod]
        public void Scan_ShouldThrowException_WhenCheckoutProcessClosed()
        {
            // Arrange
            var service = GetDefaultService();
            Action scanAction = () => service.Scan(ValidCode);
            service.Start();
            service.Close();

            // Act & Assert
            scanAction.Should().Throw<InvalidOperationException>();
        }

        private static CheckoutService GetDefaultService()
        {
            var resolver = new RegisteringResolver();
            var repository = Substitute.For<IProductRepository>();
            repository.FindBy(new BarCode(ValidCode)).Returns(new Product(TestProductName, 0.98M));
            resolver.Register<IProductRepository>(() => repository);

            return new CheckoutService(resolver);
        }
    }
}
