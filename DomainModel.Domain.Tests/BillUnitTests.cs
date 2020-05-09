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
        private static Func<Product> CreateProductPear = () => new Product("körte", 0.2M);
        private static Func<Product> CreateProductApple = () => new Product("alma", 0.3M);
        private static Func<Product> CreateProductWalnut = () => new Product("dió", 0.4M);

        private static readonly BarCode ValidBarCode = new BarCode("123");
        private const string NoBillText = "No bill available!";
        private const string EmptyBillText = "Empty bill - nothing bought.";

        [TestMethod]
        public void PrintableLastScannedProductText_ReturnsNoBillTextIfNoBillExists()
        {
            // Arrange
            var bill = Bill.NoBill;

            // Act
            var print = bill.PrintableLastScannedProductText;

            // Assert
            Assert.AreEqual(NoBillText, print);
        }

        [TestMethod]
        public void PrintableLastScannedProductText_ReturnsEmptyBillTextIfNoBillExists()
        {
            // Arrange
            var bill = Bill.EmptyBill;

            // Act
            var print = bill.PrintableLastScannedProductText;

            // Assert
            Assert.AreEqual(EmptyBillText, print);
        }

        [TestMethod]
        public void PrintableLastScannedProductText_ReturnsLastScannedProductsTextWhenProductsExists()
        {
            // Arrange
            var bill = Bill.EmptyBill;

            // Act
            bill = bill.Add(CreateProductApple());
            bill = bill.Add(CreateProductWalnut());
            bill = bill.Add(CreateProductPear());
            var print = bill.PrintableLastScannedProductText;

            // Assert
            var expectation = "körte                                   € 0,20   € 0,90";
            Assert.AreEqual(expectation, print);
        }

        [TestMethod]
        public void CancelLast_ThrowsBoughtProductNotFoundExceptionIfDoesntHaveProduct()
        {
            // Arrange
            var bill = Bill.EmptyBill;
            var ProductKorte = CreateProductPear();
            Action cancelAction = () => bill.CancelLast(ProductKorte);

            // Act & Assert
            cancelAction.Should().Throw<BoughtProductNotFoundException>();
        }

        [TestMethod]
        public void CancelLast_RemovesItemIfOnTheBill()
        {
            // Arrange
            var expectations = CreateBillFromProducts(
                CreateProductApple(),
                CreateProductPear(),
                CreateProductWalnut(),
                CreateProductApple());

            var reality = CreateBillFromProducts(
                CreateProductApple(),
                CreateProductPear(),
                CreateProductWalnut(),
                CreateProductPear(),
                CreateProductApple());

            // Act
            reality = reality.CancelLast(CreateProductPear());

            // Assert
            Assert.AreEqual(expectations, reality);
        }

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
