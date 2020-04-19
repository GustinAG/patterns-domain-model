using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainModel.Domain.Tests
{
    [TestClass]
    public class HashCodeHelperUnitTests
    {
        [TestMethod]
        public void CombineHashCodes_ShouldRunWithoutException()
        {
            // Arrange
            var objects = new List<object> { 3, "apple" };

            // Act
            var hash = HashCodeHelper.CombineHashCodes(objects);
            Console.WriteLine(hash);
        }

        [TestMethod]
        public void CombineHashCodes_ShouldReturnSameValueForSameObject()
        {
            // Arrange
            var objects = new List<object> { "apple" };

            // Act
            var hash1 = HashCodeHelper.CombineHashCodes(objects);
            var hash2 = HashCodeHelper.CombineHashCodes(objects);

            // Assert
            Console.WriteLine(hash1);
            Console.WriteLine(hash2);
            hash2.Should().Be(hash1);
        }

        [TestMethod]
        public void CombineHashCodes_ShouldThrowExceptionForNull()
        {
            // Arrange
            int hash = 0;
            Action combineAction = () => hash = HashCodeHelper.CombineHashCodes(null);

            // Act & Assert
            combineAction.Should().Throw<ArgumentException>("objects list mustn't be null");
            Console.WriteLine(hash);
        }
    }
}
