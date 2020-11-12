using System;
using Checkout.Contracts;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using Checkout.Infrastructure;
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
        private const string BeerCode = "12";
        private const decimal Limit = 1M;

        private static readonly Product TestProduct = new Product(TestProductName, 0.98M);

        [TestMethod]
        public void Start_ShouldProduceEmptyBill()
        {
            // Arrange
            var service = CreateService();

            // Act
            service.Start();

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
            service.Start();

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
        public void Scan_ShouldProduceOneGroup_WhenSameProductScanned()
        {
            // Arrange
            var service = CreateService();
            service.Start();

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
            service.Start();

            // Act & Assert
            scanAction.Should().Throw<InvalidBarCodeException>();
        }

        [TestMethod]
        public void Scan_ShouldThrowException_WhenCheckoutProcessClosed()
        {
            // Arrange
            var service = CreateService();
            Action scanAction = () => service.Scan(ValidCode);
            service.Start();
            service.Close();

            // Act & Assert
            scanAction.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Scan_ShouldWarn_WhenLimitExceeded()
        {
            // Arrange
            var presenter = Substitute.For<IWarningPresenter>();
            var service = CreateService(true, presenter);
            service.Start();
            service.SetUpLimit(Limit);

            // Act
            service.Scan(ValidCode);
            service.Scan(ValidCode);

            // Assert
            presenter.Received(1).ShowWarning(Arg.Any<string>());
        }

        [TestMethod]
        public void Scan_ShouldNotWarn_WhenLimitNotExceeded()
        {
            // Arrange
            var presenter = Substitute.For<IWarningPresenter>();
            var service = CreateService(true, presenter);
            service.Start();
            service.SetUpLimit(Limit);

            // Act
            service.Scan(ValidCode);

            // Assert
            presenter.DidNotReceive().ShowWarning(Arg.Any<string>());
        }

        [TestMethod]
        public void Scan_ShouldWarnTwice_WhenLimitExceededTwice()
        {
            // Arrange
            var presenter = Substitute.For<IWarningPresenter>();
            var service = CreateService(true, presenter);
            service.Start();
            service.SetUpLimit(Limit);

            // Act
            service.Scan(ValidCode);
            service.Scan(ValidCode);
            service.Scan(ValidCode);

            // Assert
            presenter.Received(2).ShowWarning(Arg.Any<string>());
        }

        [TestMethod]
        public void Scan_ShouldWarn_WhenAdultProduct()
        {
            // Arrange
            var presenter = Substitute.For<IWarningPresenter>();
            var service = CreateService(false, presenter);
            service.Start();

            // Act
            service.Scan(BeerCode);

            // Assert
            presenter.Received(1).ShowWarning(Arg.Any<string>());
        }

        private static CheckoutService CreateService(bool mockRepository = true, IWarningPresenter presenter = null)
        {
            var events = new DomainEvents();
            var repository = mockRepository ? CreateMockedRepository() : new ProductRepository();
            presenter ??= Substitute.For<IWarningPresenter>();
            return new CheckoutService(events, new OutChecker(repository, events), presenter);
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
