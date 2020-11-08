using System.Linq;
using Checkout.Contracts;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Checkout.Terminal.Tests
{
    [TestClass]
    public class BillPresenterUnitTests
    {
        private static readonly Product Apple = new Product("apple", 0.3M);
        private static readonly Product Pear = new Product("pear", 0.5M);

        [TestMethod]
        public void ShowClosedBill_ShouldContainDashedLine()
        {
            // Arrange
            var bill = CreateBillFromProducts(Apple, Pear);
            var service = Substitute.For<ICheckoutService>();
            service.GetCurrentBill().Returns(bill);
            var presenter = new BillPresenter(service);

            // Based on: https://www.codeproject.com/articles/501610/getting-console-output-within-a-unit-test
            using var consoleOutput = new ConsoleOutput();

            // Act
            presenter.ShowClosedBill();

            // Assert
            var text = consoleOutput.GetOutput();
            text.Should().Contain("-------------");
        }

        private static Bill CreateBillFromProducts(params Product[] products) => products.Aggregate(Bill.EmptyBill, (b, p) => b.AddOne(p));
    }
}
