using System;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DomainModel.Domain.Tests
{
    [TestClass]
    public class OutCheckerUnitTests
    {
        private static readonly BarCode ValidBarCode = new BarCode("123");
        private static readonly BarCode InvalidBarCode = new BarCode("000");

        [TestMethod]
        public void Start_ShouldRunWithoutException()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();

            // Act
            outChecker.Start();
        }

        [TestMethod]
        public void Scan_ShouldRunWithoutException()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();
            outChecker.Start();

            // Act
            outChecker.Scan(ValidBarCode);
        }

        [TestMethod]
        public void Scan_ShouldThrowException_WhenCheckoutNotStartedYet()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();
            Action scanAction = () => outChecker.Scan(ValidBarCode);

            // Act & Assert
            scanAction.Should().Throw<InvalidOperationException>("mustn't scan before checkout started");
        }

        [TestMethod]
        public void Scan_ShouldThrowExceptionForInvalidBarCode()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();
            Action scanAction = () => outChecker.Scan(InvalidBarCode);
            outChecker.Start();

            // Act & Assert
            scanAction.Should().Throw<InvalidBarCodeException>("invalid bar code should be indicated");
        }

        [TestMethod]
        public void ShowBill_ShouldGiveNoBill_WhenCheckoutNotStartedYet()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();

            // Act
            var bill = outChecker.ShowBill();

            // Assert
            bill.Should().Be(Bill.NoBill);
        }

        [TestMethod]
        public void ShowBill_ShouldGiveEmptyBill_WhenNothingScannedYet()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();
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
            var outChecker = CreateDefaultOutChecker();
            outChecker.Start();
            outChecker.Scan(ValidBarCode);

            // Act
            var bill = outChecker.ShowBill();

            // Assert
            Console.WriteLine(bill.PrintableText);
            bill.PrintableText.Should().NotBeEquivalentTo(Bill.EmptyBill.PrintableText);
            bill.PrintableText.Should().NotBeEquivalentTo(Bill.NoBill.PrintableText);
        }

        [TestMethod]
        public void ShowBill_ShouldContainDashedLine()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();
            outChecker.Start();
            outChecker.Scan(ValidBarCode);

            // Act
            var bill = outChecker.ShowBill();

            // Assert
            Console.WriteLine(bill.PrintableText);
            bill.PrintableText.Should().Contain("-------------");
        }

        [TestMethod]
        public void ShowBill_ShouldContainSummaryLine()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();
            outChecker.Start();
            outChecker.Scan(ValidBarCode);

            // Act
            var bill = outChecker.ShowBill();

            // Assert
            Console.WriteLine(bill.PrintableText);
            bill.PrintableText.Should().Contain("TOTAL");
        }

        [TestMethod]
        public void Close_ShouldRunWithoutException()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();
            outChecker.Start();
            outChecker.Scan(ValidBarCode);

            // Act
            outChecker.Close();
        }

        [TestMethod]
        public void Close_ShouldThrowException_WhenCheckoutNotStartedYet()
        {
            // Arrange
            var outChecker = CreateDefaultOutChecker();
            Action closeAction = () => outChecker.Close();

            // Act & Assert
            closeAction.Should().Throw<InvalidOperationException>("cannot close before before start");
        }

        private static OutChecker CreateDefaultOutChecker()
        {
            var repository = Substitute.For<IProductRepository>();
            repository.FindBy(ValidBarCode).Returns(new Product("unit-test-product", 0.89M));
            repository.FindBy(InvalidBarCode).Returns(Product.NoProduct);
            return new OutChecker(repository);
        }
    }
}
