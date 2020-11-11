using System;
using System.Globalization;
using System.Linq;
using Checkout.Domain.Checkout;
using Checkout.Domain.Discounts;
using Checkout.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using static System.FormattableString;

namespace Checkout.Presentation.Tests
{
    [TestClass]
    public class BillAppearanceUnitTests
    {
        private const string AppleName = "apple";
        private const string PearName = "pear";
        private const decimal ApplePrice = 0.3M;
        private const decimal PearPrice = 0.2M;
        private const decimal WalnutPrice = 0.4M;

        private static readonly Product Apple = new Product(AppleName, ApplePrice);
        private static readonly Product Pear = new Product(PearName, PearPrice);
        private static readonly Product Walnut = new Product("walnut", WalnutPrice);

        [TestMethod]
        public void AsText_ShouldContainTotalPriceInLastLine()
        {
            // Arrange
            var bill = CreateBillFromProducts(Apple, Pear);
            var appearance = new BillAppearance(bill);
            var expectedTotalPriceText = (ApplePrice + PearPrice).ToString(CultureInfo.InvariantCulture);

            // Act
            var text = appearance.AsText;

            // Assert
            LastLineOf(text).Should().Contain(expectedTotalPriceText);
        }

        [TestMethod]
        public void AsText_ShouldContainDashedLine()
        {
            // Arrange
            var bill = CreateBillFromProducts(Apple, Pear);
            var appearance = new BillAppearance(bill);

            // Act
            var text = appearance.AsText;

            // Assert
            Console.WriteLine(text);
            text.Should().Contain("-------------");
        }

        [TestMethod]
        public void AsText_ShouldContainSummaryLine()
        {
            // Arrange
            var bill = CreateBillFromProducts(Apple, Pear);
            var appearance = new BillAppearance(bill);

            // Act
            var text = appearance.AsText;

            // Assert
            Console.WriteLine(text);
            text.Should().Contain("TOTAL");
        }

        [TestMethod]
        public void AsText_ShouldContainVolumeDiscount()
        {
            // Arrange
            var bill = CreateVolumeBill();

            // Act
            var appearance = new BillAppearance(bill);

            // Act
            var text = appearance.AsText;

            // Assert
            Console.WriteLine(text);
            text.Should().Contain($"{AppleName} volume discount");
        }

        [TestMethod]
        public void AsText_ShouldContainVolumeDiscountUnderProduct()
        {
            // Arrange
            var bill = CreateVolumeBill();

            // Act
            var appearance = new BillAppearance(bill);

            // Act
            var text = appearance.AsText;

            // Assert
            Console.WriteLine(text);
            SecondLineOf(text).Should().Contain($"{AppleName} volume discount");
        }

        [TestMethod]
        public void LastAddedProductAsText_ShouldReturnVeryShortText_WhenNoBillExists()
        {
            // Arrange
            var bill = Bill.NoBill;
            var appearance = new BillAppearance(bill);

            // Act
            var text = appearance.LastAddedProductAsText;

            // Assert
            text.Length.Should().BeLessOrEqualTo(3);
        }

        [TestMethod]
        public void LastAddedProductAsText_ShouldReturnVeryShortText_WhenNoProductOnBillYet()
        {
            // Arrange
            var bill = Bill.EmptyBill;
            var appearance = new BillAppearance(bill);

            // Act
            var text = appearance.LastAddedProductAsText;

            // Assert
            text.Length.Should().BeLessOrEqualTo(3);
        }

        [TestMethod]
        public void LastAddedProductAsText_ShouldContainLastAddedProductDetails()
        {
            // Arrange
            var bill = CreateBillFromProducts(Apple, Walnut, Pear);
            var expectedPearPriceText = Invariant($"€ {PearPrice}");
            var appearance = new BillAppearance(bill);

            // Act
            var text = appearance.LastAddedProductAsText;

            // Assert
            Console.WriteLine(text);
            text.Should().Contain(PearName);
            text.Should().Contain(expectedPearPriceText);
        }

        private static Bill CreateBillFromProducts(params Product[] products) => products.Aggregate(Bill.EmptyBill, (b, p) => b.AddOne(p));

        private static Bill CreateVolumeBill()
        {
            var apples = Enumerable.Repeat(Apple, 5);
            var pears = Enumerable.Repeat(Pear, 5);
            var products = apples.Concat(pears).ToArray();

            var repository = Substitute.For<IProductRepository>();
            repository.FindBy(Arg.Any<string>()).Returns(Walnut);   // So that we don't get any combined discount here

            return CreateBillFromProducts(products).ApplyDiscounts(new Discounter(repository));
        }

        private static string LastLineOf(string text) => text.Split(Environment.NewLine).Last();

        private static string SecondLineOf(string text) => text.Split(Environment.NewLine).Skip(1).FirstOrDefault();
    }
}
