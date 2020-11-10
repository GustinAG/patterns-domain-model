using System;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Checkout.Domain.Tests
{
    [TestClass]
    public class OutCheckerUnitTests
    {
        private static readonly BarCode ValidBarCode = new BarCode("123");
        private static readonly BarCode InvalidBarCode = new BarCode("000");

        [TestMethod]
        public void Start_ShouldAllowScan()
        {
            // Arrange
            var outChecker = CreateOutChecker();

            // Act
            outChecker.Start();

            // Assert
            outChecker.CanScan.Should().BeTrue();
        }

        [TestMethod]
        public void Scan_ShouldAllowCancel()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            outChecker.Start();

            // Act
            outChecker.Scan(ValidBarCode);

            // Assert
            outChecker.CanCancel.Should().BeTrue();
        }

        [TestMethod]
        public void Scan_ShouldThrowException_WhenCheckoutNotStartedYet()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            Action scanAction = () => outChecker.Scan(ValidBarCode);

            // Act & Assert
            scanAction.Should().Throw<InvalidOperationException>("mustn't scan before checkout started");
        }

        [TestMethod]
        public void Scan_ShouldThrowException_WhenInvalidBarCode()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            Action scanAction = () => outChecker.Scan(InvalidBarCode);
            outChecker.Start();

            // Act & Assert
            scanAction.Should().Throw<InvalidBarCodeException>("invalid bar code should be indicated");
        }

        [TestMethod]
        public void Scan_ShouldProduceLowerTotalPrice_WhenDiscountApplies()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            outChecker.Start();

            // Act
            outChecker.Scan(ValidBarCode);
            outChecker.Scan(ValidBarCode);
            outChecker.Scan(ValidBarCode);
            outChecker.Scan(ValidBarCode);
            outChecker.Scan(ValidBarCode);

            // Assert
            var bill = outChecker.ShowBill();
            Console.WriteLine(bill);
            bill.TotalPrice.Should().BeLessThan(bill.NoDiscountTotalPrice);
        }

        [TestMethod]
        public void ShowBill_ShouldGiveNoBill_WhenCheckoutNotStartedYet()
        {
            // Arrange
            var outChecker = CreateOutChecker();

            // Act
            var bill = outChecker.ShowBill();

            // Assert
            bill.Should().Be(Bill.NoBill);
        }

        [TestMethod]
        public void ShowBill_ShouldGiveEmptyBill_WhenNothingScannedYet()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            outChecker.Start();

            // Act
            var bill = outChecker.ShowBill();

            // Assert
            bill.Should().Be(Bill.EmptyBill);
        }

        [TestMethod]
        public void ShowBill_ShouldBeDifferent_WhenSomethingScanned()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            outChecker.Start();
            outChecker.Scan(ValidBarCode);

            // Act
            var bill = outChecker.ShowBill();

            // Assert
            Console.WriteLine(bill);
            bill.Should().NotBeEquivalentTo(Bill.EmptyBill);
            bill.Should().NotBeEquivalentTo(Bill.NoBill);
        }

        [TestMethod]
        public void Close_ShouldDisableScan()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            outChecker.Start();
            outChecker.Scan(ValidBarCode);

            // Act
            outChecker.Close();

            // Assert
            outChecker.CanScan.Should().BeFalse();
        }

        [TestMethod]
        public void Close_ShouldThrowException_WhenCheckoutNotStartedYet()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            Action closeAction = () => outChecker.Close();

            // Act & Assert
            closeAction.Should().Throw<InvalidOperationException>("cannot close before before start");
        }

        private static OutChecker CreateOutChecker()
        {
            var collector = Substitute.For<IDomainEventCollector>();
            var repository = CreateMockedProductRepository();
            return new OutChecker(repository, collector);
        }

        private static IProductRepository CreateMockedProductRepository()
        {
            var repository = Substitute.For<IProductRepository>();

            repository.FindBy(ValidBarCode).Returns(new Product("unit-test-product", 0.89M));
            repository.FindBy(InvalidBarCode).Returns(Product.NoProduct);
            repository.FindBy(Arg.Any<string>()).Returns(Product.NoProduct);

            return repository;
        }
    }
}
