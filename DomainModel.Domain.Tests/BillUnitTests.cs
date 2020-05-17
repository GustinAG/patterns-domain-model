using System;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Discounts;
using DomainModel.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DomainModel.Domain.Tests
{
    [TestClass]
    public class BillUnitTests
    {
        private static readonly BarCode ValidBarCode = new BarCode("123");
        private const string NoBillText = "No bill available!";
        private const string EmptyBillText = "Empty bill - nothing bought.";

        [TestMethod]
        public void PrintableLastAddedProductText_ReturnsNoBillTextIfNoBillExists()
        {
            // Arrange
            var bill = Bill.NoBill;

            // Act
            var text = bill.PrintableLastAddedProductText;

            // Assert
            text.Should().Be(NoBillText);
        }

        [TestMethod]
        public void PrintableLastAddedProductText_ReturnsEmptyBillTextIfNoBillExists()
        {
            // Arrange
            var bill = Bill.EmptyBill;

            // Act
            var text = bill.PrintableLastAddedProductText;

            // Assert
            text.Should().Be(EmptyBillText);
        }

        [TestMethod]
        public void PrintableLastAddedProductText_ReturnsLastAddedProductsTextWhenProductsExists()
        {
            // Arrange
            var bill = Bill.EmptyBill;

            // Act
            bill = bill.Add(CreateProductApple());
            bill = bill.Add(CreateProductWalnut());
            bill = bill.Add(CreateProductPear());
            var text = bill.PrintableLastAddedProductText;

            // Assert
            text.Should().Contain("pear");
            text.Should().Contain("€ 0,20");
            text.Should().Contain("€ 0,90");
        }

        [TestMethod]
        public void CancelOne_ThrowsBoughtProductNotFoundExceptionIfDoesntHaveProduct()
        {
            // Arrange
            var bill = Bill.EmptyBill;
            var ProductKorte = CreateProductPear();
            Action cancelAction = () => bill.CancelOne(ProductKorte);

            // Act & Assert
            cancelAction.Should().Throw<BoughtProductNotFoundException>();
        }

        [TestMethod]
        public void CancelOne_RemovesItemIfOnTheBill()
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
        private static Product  CreateProductWalnut() => new Product("walnut", 0.4M);

        private static Bill CreateBillFromProducts(params Product[] products)
        {
            var bill = Bill.EmptyBill;
            foreach (var product in products)
            {
                bill = bill.Add(product);
            }

            return bill;
        }
    }
}
