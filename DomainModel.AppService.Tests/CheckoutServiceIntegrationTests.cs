﻿using System;
using DomainModel.Domain.Checkout;
using DomainModel.Domain.Products;
using DomainModel.Repositories;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DomainModel.AppService.Tests
{
    [TestClass]
    public class CheckoutServiceIntegrationTests
    {
        private const string TestProductName = "Test Product";
        private const string ValidCode = "1234";
        private const string InvalidCode = "000";

        private static readonly Action<decimal, decimal> EmptyAction = (a, b) => { };

        [TestMethod]
        public void Start_ShouldProduceEmptyBill()
        {
            // Arrange
            var service = CreateService();

            // Act
            service.Start(EmptyAction);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().Be(Bill.EmptyBill.PrintableText);
        }

        [TestMethod]
        public void Scan_ShouldProduceNonEmptyBill()
        {
            // Arrange
            var service = CreateService();
            service.Start(EmptyAction);

            // Act
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().NotBeEquivalentTo(Bill.EmptyBill.PrintableText);
        }

        [TestMethod]
        public void Scan_ShouldProduceBillWithScannedProduct()
        {
            // Arrange
            var service = CreateService();
            service.Start(EmptyAction);

            // Act
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().Contain(TestProductName);
        }

        [TestMethod]
        public void Scan_ShouldProduceThreeLinesBill_WhenSameProductScanned()
        {
            // Arrange
            var service = CreateService();
            service.Start(EmptyAction);

            // Act
            service.Scan(ValidCode);
            service.Scan(ValidCode);
            service.Scan(ValidCode);

            // Assert
            var currentBill = service.GetCurrentBill();
            Console.WriteLine(currentBill);
            currentBill.Should().Contain(Environment.NewLine, Exactly.Twice());
        }

        [TestMethod]
        public void Scan_ShouldThrowExceptionForInvalidBarCode()
        {
            // Arrange
            var service = CreateService(false);
            Action scanAction = () => service.Scan(InvalidCode);
            service.Start(EmptyAction);

            // Act & Assert
            scanAction.Should().Throw<InvalidBarCodeException>();
        }

        [TestMethod]
        public void Scan_ShouldThrowException_WhenCheckoutProcessClosed()
        {
            // Arrange
            var service = CreateService();
            Action scanAction = () => service.Scan(ValidCode);
            service.Start(EmptyAction);
            service.Close();

            // Act & Assert
            scanAction.Should().Throw<InvalidOperationException>();
        }

        private static CheckoutService CreateService(bool mockRepository = true)
        {
            var repository = mockRepository ? CreateMockedRepository() : new ProductRepository();
            return new CheckoutService(new OutChecker(repository));
        }

        private static IProductRepository CreateMockedRepository()
        {
            var repository = Substitute.For<IProductRepository>();

            repository.FindBy(new BarCode(ValidCode)).Returns(new Product(TestProductName, 0.98M));
            repository.FindBy(Arg.Any<string>()).Returns(Product.NoProduct);

            return repository;
        }
    }
}
