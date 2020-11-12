using Checkout.Contracts;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using Checkout.Presentation.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Checkout.Presentation.Tests
{
    [TestClass]
    public class InvokerUnitTests
    {
        [TestMethod]
        public void Invoke_ShouldShowError_WhenAdultProductNotAllowedException()
        {
            // Arrange
            var presenter = Substitute.For<IPresenter>();
            var service = Substitute.For<ICheckoutService>();
            service.When(s => s.Scan(Arg.Any<string>())).Do(c => throw new AdultProductBuyingNotAllowedException(Customer.Unknown, new Product("test-product", 0.9M)));
            var invoker = new Invoker(presenter, service);

            // Act
            invoker.Invoke(new ScanCommand(service));

            // Assert
            presenter.Received().ShowError(Arg.Any<string>());
        }
    }
}
