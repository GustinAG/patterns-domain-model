using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Checkout.Domain.Checkout;
using Checkout.Domain.Discounts;
using Checkout.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using static System.FormattableString;

namespace Checkout.Domain.Tests
{
    [TestClass]
    public class BillUnitTests
    {
        private const string NoBillText = "No bill available!";
        private const string EmptyBillText = "Empty bill - nothing bought.";
        private const decimal ApplePrice = 0.3M;
        private const decimal PearPrice = 0.2M;
        private const string PearName = "pear";
        private const decimal WalnutPrice = 0.4M;

        [TestMethod]
        public void PrintableText_ShouldContainSubtotalValueInLastLine()
        {
            // Arrange
            var bill = CreateBillFromProducts(CreateProductApple(), CreateProductPear());
            var expectedSubtotalText = (ApplePrice + PearPrice).ToString(CultureInfo.InvariantCulture);

            // Act
            var text = bill.PrintableText;

            // Assert
            LastLineOf(text).Should().Contain(expectedSubtotalText);
        }

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
            var bill = CreateBillFromProducts(CreateProductApple(), CreateProductWalnut(), CreateProductPear());
            var expectedPearPriceText = Invariant($"€ {PearPrice}");
            var expectedTotalPriceText = Invariant($"€ {ApplePrice + WalnutPrice + PearPrice}");

            // Act
            var text = bill.PrintableLastAddedProductText;

            // Assert
            text.Should().Contain(PearName);
            text.Should().Contain(expectedPearPriceText);
            text.Should().Contain(expectedTotalPriceText);
        }

        [TestMethod]
        public void ApplyDiscounts_ShouldDecreaseTotalPrice()
        {
            // Arrange
            var count = 5;
            var products = Enumerable.Repeat(CreateProductApple(), count).ToArray();
            var totalPriceWithoutDiscount = ApplePrice * count;
            var bill = CreateBillFromProducts(products);

            var mockedRepository = Substitute.For<IProductRepository>();
            mockedRepository.FindBy(Arg.Any<string>()).Returns(Product.NoProduct);

            // Act
            bill = bill.ApplyDiscounts(new Discounter(mockedRepository));

            // Assert
            ExtractFirstDecimalFromText(LastLineOf(bill.PrintableText)).Should().BeLessThan(totalPriceWithoutDiscount);
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
            var expectedBill = CreateBillFromProducts(
                CreateProductApple(),
                CreateProductWalnut(),
                CreateProductPear(),
                CreateProductApple()
            );

            var bill = CreateBillFromProducts(
                CreateProductApple(),
                CreateProductPear(),
                CreateProductWalnut(),
                CreateProductPear(),
                CreateProductApple());

            // Act
            bill = bill.CancelOne(CreateProductPear());

            // Assert
            bill.Should().Be(expectedBill);
        }

        private static Product CreateProductPear() => new Product(PearName, PearPrice);
        private static Product CreateProductApple() => new Product("apple", ApplePrice);
        private static Product CreateProductWalnut() => new Product("walnut", WalnutPrice);

        private static Bill CreateBillFromProducts(params Product[] products) => products.Aggregate(Bill.EmptyBill, (b, p) => b.AddOne(p));

        private static string LastLineOf(string text) => text.Split(Environment.NewLine).Last();

        // https://stackoverflow.com/questions/4734116/find-and-extract-a-number-from-a-string
        private static decimal ExtractFirstDecimalFromText(string text)
        {
            var extractedString = Regex.Match(text, @"\d+\.\d+").Value;
            extractedString.Should().NotBeNullOrWhiteSpace();
            return decimal.Parse(extractedString, CultureInfo.InvariantCulture);
        }
    }
}
