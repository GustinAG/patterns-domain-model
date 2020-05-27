using System;
using System.Linq;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainModel.Domain.Tests
{
    [TestClass]
    public class BillUnitTests
    {
        private const string NoBillText = "No bill available!";
        private const string EmptyBillText = "Empty bill - nothing bought.";

        [TestMethod]
        public void PrintableLastAddedProductText_ShouldReturnNoBillText_WhenNoBillExists()
        {
            // Arrange
            var bill = Bill.NoBill;

            // Act
            var text = bill.PrintableLastAddedProductText;

            // Assert
            text.Should().Be(NoBillText);
        }

        [TestMethod]
        public void PrintableLastAddedProductText_ShouldReturnEmptyBillText_WhenNoProductOnBillYet()
        {
            // Arrange
            var bill = Bill.EmptyBill;

            // Act
            var text = bill.PrintableLastAddedProductText;

            // Assert
            text.Should().Be(EmptyBillText);
        }

        [TestMethod]
        public void PrintableLastAddedProductText_ShouldReturnLastAddedProductDetails()
        {
            // Arrange
            var bill = Bill.EmptyBill;
            bill = bill.Add(CreateProductApple());
            bill = bill.Add(CreateProductWalnut());
            bill = bill.Add(CreateProductPear());

            // Act
            var text = bill.PrintableLastAddedProductText;

            // Assert
            text.Should().Contain("pear");
            text.Should().Contain("€ 0.20");
            text.Should().Contain("€ 0.90");
        }

        [TestMethod]
        public void CancelOne_ShouldThrowBoughtProductNotFoundException_WhenProductNotOnBill()
        {
            // Arrange
            var bill = Bill.EmptyBill;
            var productPear = CreateProductPear();
            Action cancelAction = () => bill.CancelOne(productPear);

            // Act & Assert
            cancelAction.Should().Throw<BoughtProductNotFoundException>();
        }

        [TestMethod]
        public void CancelOne_ShouldRemoveOneProductFromBill()
        {
            // Arrange
            var expectations = CreateBillFromProducts(
                CreateProductApple(),
                CreateProductWalnut(),
                CreateProductPear(),
                CreateProductApple()
            );

            var reality = CreateBillFromProducts(
                CreateProductApple(),
                CreateProductPear(),
                CreateProductWalnut(),
                CreateProductPear(),
                CreateProductApple());

            // Act
            reality = reality.CancelOne(CreateProductPear());

            // Assert
            reality.Should().Be(expectations);
        }

        private static Product CreateProductPear() => new Product("pear", 0.2M);
        private static Product CreateProductApple() => new Product("apple", 0.3M);
        private static Product CreateProductWalnut() => new Product("walnut", 0.4M);

        private static Bill CreateBillFromProducts(params Product[] products) => products.Aggregate(Bill.EmptyBill, (b, p) => b.Add(p));
    }
}
