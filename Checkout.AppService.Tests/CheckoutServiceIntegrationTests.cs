using System;
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
        private const decimal Limit = 1M;

        private static readonly Product TestProduct = new Product(TestProductName, 0.98M);

        private int _limitExceededCount;
        private decimal _exceededLimit;

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
        public void Scan_ShouldCallLimitExceededActionWithPresetLimit()
        {
            // Arrange
            var service = CreateService();
            service.Start(TrackLimitExceededCalls);
            service.SetUpLimit(Limit);

            // Act
            service.Scan(ValidCode);
            service.Scan(ValidCode);

            // Assert
            _limitExceededCount.Should().Be(1);
            _exceededLimit.Should().Be(Limit);
        }

        [TestMethod]
        public void Scan_ShouldNotCallLimitExceededAction_WhenUnderPresetLimit()
        {
            // Arrange
            var service = CreateService();
            service.Start(TrackLimitExceededCalls);
            service.SetUpLimit(Limit);

            // Act
            service.Scan(ValidCode);

            // Assert
            _limitExceededCount.Should().Be(0);
        }

        [TestMethod]
        public void Scan_ShouldCallLimitExceededActionTwice_WhenOccursTwice()
        {
            // Arrange
            var service = CreateService();
            service.Start(TrackLimitExceededCalls);
            service.SetUpLimit(Limit);

            // Act
            service.Scan(ValidCode);
            service.Scan(ValidCode);
            service.Scan(ValidCode);

            // Assert
            _limitExceededCount.Should().Be(2);
            _exceededLimit.Should().Be(Limit);
        }

        private static CheckoutService CreateService(bool mockRepository = true)
        {
            var events = new DomainEvents();
            var repository = mockRepository ? CreateMockedRepository() : new ProductRepository();
            return new CheckoutService(events, new OutChecker(repository, events));
        }

        private static IProductRepository CreateMockedRepository()
        {
            var repository = Substitute.For<IProductRepository>();

            repository.FindBy(new BarCode(ValidCode)).Returns(TestProduct);
            repository.FindBy(Arg.Any<string>()).Returns(Product.NoProduct);

            return repository;
        }

        private void TrackLimitExceededCalls(CheckoutLimitExceeded e)
        {
            _limitExceededCount++;
            _exceededLimit = e.Limit.Limit;
        }
    }
}
