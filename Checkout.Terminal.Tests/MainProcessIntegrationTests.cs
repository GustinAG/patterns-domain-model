using Autofac;
using Checkout.Domain.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Checkout.Terminal.Tests
{
    [TestClass]
    public class MainProcessIntegrationTests
    {
        private const string TestBarCode = "111";

        [TestMethod]
        public void Run_ShouldCallRepository()
        {
            // Arrange
            const string testProductName = "test";
            var testProduct = new Product(testProductName, decimal.One);
            var repository = CreateMockedRepository(testProduct);
            var container = TypeRegistry.Build(b => RegisterMocks(b, repository));

            using var scope = container.BeginLifetimeScope();
            var process = scope.Resolve<MainProcess>();

            // Act
            process.Run();

            // Assert
            repository.Received().FindBy(Arg.Any<string>());
        }

        private static void RegisterMocks(ContainerBuilder builder, IProductRepository repository)
        {
            var reader = Substitute.For<ICommandReader>();
            reader.ReadCommandCode().Returns(TestBarCode, CommandCode.Exit);
            builder.RegisterInstance(reader).As<ICommandReader>();

            builder.RegisterInstance(repository).As<IProductRepository>().SingleInstance();
        }

        private static IProductRepository CreateMockedRepository(Product testProduct)
        {
            var repository = Substitute.For<IProductRepository>();
            repository.FindBy(new BarCode(TestBarCode)).Returns(testProduct);
            repository.FindBy(Arg.Any<string>()).Returns(i => new Product(i[0].ToString(), decimal.One));
            return repository;
        }
    }
}
