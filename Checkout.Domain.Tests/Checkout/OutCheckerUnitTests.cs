using System;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Checkout.Domain.Tests.Checkout
{
    [TestClass]
    public class OutCheckerUnitTests
    {
        private static readonly BarCode ValidBarCode = new BarCode("123");
        private static readonly BarCode InvalidBarCode = new BarCode("000");
        private static readonly BarCode AdultBarCode = new BarCode("999");

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
        public void Scan_ShouldThrowException_WhenUnknownCustomerBuyingAdultProduct()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            Action scanAction = () => outChecker.Scan(AdultBarCode);
            outChecker.Start();

            // Act & Assert
            scanAction.Should().Throw<AdultProductBuyingNotAllowedException>("customer not validated yet");
        }

        [TestMethod]
        public void Scan_ShouldThrowException_WhenChildCustomerBuyingAdultProduct()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            Action scanAction = () => outChecker.Scan(AdultBarCode);
            outChecker.Start();

            var birthDate = DateTime.Now.AddYears(-10);
            outChecker.SetCustomerBirthDate(new BirthDate(birthDate));

            // Act & Assert
            scanAction.Should().Throw<AdultProductBuyingNotAllowedException>("customer too young");
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
        public void Scan_ShouldRaiseLimitExceededEvent()
        {
            // Arrange
            var collector = Substitute.For<IDomainEventCollector>();
            var outChecker = CreateOutChecker(collector);
            outChecker.Start();
            outChecker.SetUpLimit(new CheckoutLimit(1M));

            // Act
            outChecker.Scan(ValidBarCode);
            outChecker.Scan(ValidBarCode);

            // Assert
            collector.Received().Raise(Arg.Any<CheckoutLimitExceeded>());
        }

        [TestMethod]
        public void Scan_ShouldRaiseAdultProductAddedToBillEvent()
        {
            // Arrange
            var collector = Substitute.For<IDomainEventCollector>();
            var outChecker = CreateOutChecker(collector);
            outChecker.Start();
            outChecker.SetCustomerBirthDate(new BirthDate(DateTime.Now.AddYears(-20)));

            // Act
            outChecker.Scan(AdultBarCode);

            // Assert
            collector.Received().Raise(Arg.Any<AdultProductAddedToBill>());
        }

        [TestMethod]
        public void Cancel_ShouldRemoveAffectedDiscount()
        {
            // Arrange
            var outChecker = CreateOutChecker();
            outChecker.Start();
            outChecker.Scan(ValidBarCode);
            outChecker.Scan(ValidBarCode);
            outChecker.Scan(ValidBarCode);
            outChecker.Scan(ValidBarCode);

            var bill = outChecker.ShowBill();
            bill.TotalPrice.Should().BeLessThan(bill.NoDiscountTotalPrice);

            // Act
            outChecker.Cancel(ValidBarCode);

            // Assert
            bill = outChecker.ShowBill();
            Console.WriteLine(bill);
            bill.TotalPrice.Should().Be(bill.NoDiscountTotalPrice);
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

        private static OutChecker CreateOutChecker(IDomainEventCollector collector = null)
        {
            collector ??= Substitute.For<IDomainEventCollector>();
            var repository = CreateMockedProductRepository();
            return new OutChecker(repository, collector);
        }

        private static IProductRepository CreateMockedProductRepository()
        {
            var repository = Substitute.For<IProductRepository>();

            repository.FindBy(ValidBarCode).Returns(new Product("unit-test-product", 0.89M));
            repository.FindBy(InvalidBarCode).Returns(Product.NoProduct);
            repository.FindBy(AdultBarCode).Returns(new Product("unit-test-adult-product", 1.33M, true));
            repository.FindBy(Arg.Any<string>()).Returns(Product.NoProduct);

            return repository;
        }
    }
}
