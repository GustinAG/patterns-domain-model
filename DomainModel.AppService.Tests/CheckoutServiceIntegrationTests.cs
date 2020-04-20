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
        private const string InvalidCode = "000";

        [TestMethod]
        public void Start_ShouldProduceEmptyBill()
        {
            // Arrange
            var service = GetServiceWithMockedRepository();

            // Act
            service.Start();

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().Be(Bill.EmptyBill.PrintableText);
        }

        [TestMethod]
        public void Scan_ShouldProduceNonEmptyBill()
        {
            // Arrange
            var service = new CheckoutService();
            service.Start();

            // Act
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().NotBeEquivalentTo(Bill.EmptyBill.PrintableText);
        }

        [TestMethod]
        public void Scan_ShouldProduceBillWithScannedProduct()
        {
            // Arrange
            var service = GetServiceWithMockedRepository();
            service.Start();

            // Act
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().Contain(TestProductName);
        }

        [TestMethod]
        public void Scan_ShouldProduceThreeLinesBill_WhenSameProductScanned()
        {
            // Arrange
            var service = new CheckoutService();
            service.Start();

            // Act
            service.Scan(ValidCode);
            service.Scan(ValidCode);
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().Contain(Environment.NewLine, Exactly.Twice());
        }

        [TestMethod]
        public void Scan_ShouldThrowExceptionForInvalidBarCode()
        {
            // Arrange
            var service = new CheckoutService();
            Action scanAction = () => service.Scan(InvalidCode);
            service.Start();

            // Act & Assert
            scanAction.Should().Throw<InvalidBarCodeException>();
        }

        [TestMethod]
        public void Scan_ShouldThrowException_WhenCheckoutProcessClosed()
        {
            // Arrange
            var service = GetServiceWithMockedRepository();
            Action scanAction = () => service.Scan(ValidCode);
            service.Start();
            service.Close();

            // Act & Assert
            scanAction.Should().Throw<InvalidOperationException>();
        }

        private static CheckoutService GetServiceWithMockedRepository()
        {
            var resolver = new RegisteringResolver();
            var repository = Substitute.For<IProductRepository>();

            repository.FindBy(new BarCode(ValidCode)).Returns(new Product(TestProductName, 0.98M));
            repository.FindBy(Arg.Any<string>()).Returns(Product.NoProduct);
            resolver.Register<IProductRepository>(() => repository);

            return new CheckoutService(resolver);
        }
    }
}
