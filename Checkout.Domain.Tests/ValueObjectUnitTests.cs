﻿using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout.Domain.Tests
{
    [TestClass]
    public class ValueObjectUnitTests
    {
        [TestMethod]
        public void Equals_ShouldBeTrueForSameValues()
        {
            // Arrange
            var banana1 = new Banana("yellow", 3);
            var banana2 = new Banana("yellow", 3);
            Console.WriteLine(banana1);
            Console.WriteLine(banana2);

            // Act & Assert
            banana1.Equals(banana2).Should().BeTrue("bananas have same values");
        }

        [TestMethod]
        public void EqualsOperator_ShouldBeTrueForSameValues()
        {
            // Arrange
            var banana1 = new Banana("yellow", 3);
            var banana2 = new Banana("yellow", 3);
            Console.WriteLine(banana1);
            Console.WriteLine(banana2);

            // Act & Assert
            (banana1 == banana2).Should().BeTrue("bananas have same values");
        }

        [TestMethod]
        public void NotEqualsOperator_ShouldBeTrueForDifferentValues()
        {
            // Arrange
            var banana1 = new Banana("yellow", 3);
            var banana2 = new Banana("brown", 3);
            Console.WriteLine(banana1);
            Console.WriteLine(banana2);

            // Act & Assert
            (banana1 != banana2).Should().BeTrue("bananas have different values");
        }
    }
}
