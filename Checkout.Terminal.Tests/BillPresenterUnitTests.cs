using System.Linq;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using Checkout.Presentation;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout.Terminal.Tests
{
    [TestClass]
    public class BillPresenterUnitTests
    {
        private static readonly Product Apple = new Product("apple", 0.3M);
        private static readonly Product Pear = new Product("pear", 0.5M);

        [TestMethod]
        public void RefreshDisplay_ShouldContainBillAsText()
        {
            // Arrange
            var presenter = new BillPresenter();
            var bill = CreateBillFromProducts(Apple, Pear);
            var appearance = new BillAppearance(bill);

            // Based on: https://www.codeproject.com/articles/501610/getting-console-output-within-a-unit-test
            using var consoleOutput = new ConsoleOutput();

            // Act
            presenter.RefreshDisplay(appearance);

            // Assert
            var text = consoleOutput.GetOutput();
            text.Should().Contain(appearance.AsText);
        }

        private static Bill CreateBillFromProducts(params Product[] products) => products.Aggregate(Bill.EmptyBill, (b, p) => b.AddOne(p));
    }
}
