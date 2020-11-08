using System;
using System.Linq;
using Checkout.Domain.Checkout;
using Checkout.Domain.Discounts;
using Checkout.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Checkout.Domain.Tests
{
    [TestClass]
    public class BillUnitTests
    {
        private const decimal ApplePrice = 0.3M;
        private const decimal PearPrice = 0.2M;
        private const string PearName = "pear";
        private const decimal WalnutPrice = 0.4M;

        private static readonly Product Pear = new Product(PearName, PearPrice);
        private static readonly Product Apple = new Product("apple", ApplePrice);
        private static readonly Product Walnut = new Product("walnut", WalnutPrice);

        [TestMethod]
        public void NoDiscountTotalPrice_ShouldBeSumOfProductPrices()
        {
            // Arrange
            var bill = CreateBillFromProducts(Apple, Pear);
            var expectedPrice = ApplePrice + PearPrice;

            // Act
            var price = bill.NoDiscountTotalPrice;

            // Assert
            price.Should().Be(expectedPrice);
        }

        [TestMethod]
        public void NoDiscountTotalPrice_ShouldBeZero_WhenNoProductOnBillYet()
        {
            // Arrange
            var bill = Bill.EmptyBill;

            // Act
            var price = bill.NoDiscountTotalPrice;

            // Assert
            price.Should().Be(0);
        }

        [TestMethod]
        public void TotalPrice_ShouldBeZero_WhenNoProductOnBillYet()
        {
            // Arrange
            var bill = Bill.EmptyBill;

            // Act
            var price = bill.TotalPrice;

            // Assert
            price.Should().Be(0);
        }

        [TestMethod]
        public void LastAddedProduct_ShouldReturnLastAddedProduct()
        {
            // Arrange
            var bill = CreateBillFromProducts(Apple, Walnut, Pear);

            // Act
            var product = bill.LastAddedProduct;

            // Assert
            product.Should().Be(Pear);
        }

        [TestMethod]
        public void ApplyDiscounts_ShouldDecreaseTotalPrice()
        {
            // Arrange
            var count = 5;
            var products = Enumerable.Repeat(Apple, count).ToArray();
            var bill = CreateBillFromProducts(products);

            var mockedRepository = Substitute.For<IProductRepository>();
            mockedRepository.FindBy(Arg.Any<string>()).Returns(Product.NoProduct);

            // Act
            bill = bill.ApplyDiscounts(new Discounter(mockedRepository));

            // Assert
            bill.TotalPrice.Should().BeLessThan(bill.NoDiscountTotalPrice);
        }

        [TestMethod]
        public void CancelOne_ShouldThrowBoughtProductNotFoundException_WhenProductNotOnBill()
        {
            // Arrange
            var bill = Bill.EmptyBill;
            Action cancelAction = () => bill.CancelOne(Pear);

            // Act & Assert
            cancelAction.Should().Throw<BoughtProductNotFoundException>();
        }

        [TestMethod]
        public void CancelOne_ShouldRemoveOneProductFromBill()
        {
            // Arrange
            var bill = CreateBillFromProducts(Apple, Pear, Walnut, Pear, Apple);
            var expectedBill = CreateBillFromProducts(Apple, Walnut, Pear, Apple);

            // Act
            bill = bill.CancelOne(Pear);

            // Assert
            bill.Should().Be(expectedBill);
        }

        private static Bill CreateBillFromProducts(params Product[] products) => products.Aggregate(Bill.EmptyBill, (b, p) => b.AddOne(p));
    }
}
