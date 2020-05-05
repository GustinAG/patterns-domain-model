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
    public class BillTests
    {
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
            bill = bill.Add(new Product("alma", 0.3M));
            bill = bill.Add(new Product("dió", 0.4M));
            bill = bill.Add(new Product("körte", 0.2M));
            var print = bill.PrintableLastScannedProductText;

            // Assert
            var expectation = "körte                                   € 0,20   € 0,90";
            Assert.AreEqual(expectation, print);
        }
    }
}
