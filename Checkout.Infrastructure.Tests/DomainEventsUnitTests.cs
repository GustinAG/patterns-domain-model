using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout.Infrastructure.Tests
{
    [TestClass]
    public class DomainEventsUnitTests
    {
        private int _oneCounter;
        private int _anotherCounter;

        private DomainEvents _events;

        [TestInitialize]
        public void Init() => _events = new DomainEvents();

        [TestMethod]
        public void PlayAll_ShouldBeAbleToProcessOneHandlerForOneEvent()
        {
            // Arrange
            _events.Register<OneUnitTestEventOccurred>(HandleTenantAdded);
            _events.Raise(new OneUnitTestEventOccurred());

            // Act
            _events.PlayAll();

            // Assert
            _oneCounter.Should().Be(1, "event handler should have been called once");
        }

        [TestMethod]
        public void PlayAll_ShouldBeAbleToProcessMultipleHandlersForOneEvent()
        {
            // Arrange
            _events.Register<OneUnitTestEventOccurred>(HandleTenantAdded);
            _events.Register<OneUnitTestEventOccurred>(HandleTenantAdded);
            _events.Register<OneUnitTestEventOccurred>(HandleTenantAdded);

            _events.Raise(new OneUnitTestEventOccurred());

            // Act
            _events.PlayAll();

            // Assert
            _oneCounter.Should().Be(3, "event handler should have been called three times");
        }

        [TestMethod]
        public void PlayAll_ShouldBeAbleToProcessSingleHandlersForMultipleEvents()
        {
            // Arrange
            _events.Register<OneUnitTestEventOccurred>(HandleTenantAdded);
            _events.Register<AnotherUnitTestEventOccurred>(HandleTenantDeleted);

            _events.Raise(new OneUnitTestEventOccurred());
            _events.Raise(new AnotherUnitTestEventOccurred());

            // Act
            _events.PlayAll();

            // Assert
            _oneCounter.Should().Be(1, "one event handler should have been called once");
            _anotherCounter.Should().Be(1, "another event handler should have been called once");
        }

        [TestMethod]
        public void PlayAll_ShouldBeAbleToProcessMultipleHandlersForMultipleEvents()
        {
            // Arrange
            _events.Register<OneUnitTestEventOccurred>(HandleTenantAdded);
            _events.Register<OneUnitTestEventOccurred>(HandleTenantAdded);

            _events.Register<AnotherUnitTestEventOccurred>(HandleTenantDeleted);
            _events.Register<AnotherUnitTestEventOccurred>(HandleTenantDeleted);
            _events.Register<AnotherUnitTestEventOccurred>(HandleTenantDeleted);

            _events.Raise(new OneUnitTestEventOccurred());
            _events.Raise(new AnotherUnitTestEventOccurred());

            // Act
            _events.PlayAll();

            // Assert
            _oneCounter.Should().Be(2, "one event handler should have been called three times");
            _anotherCounter.Should().Be(3, "another event handler should have been called twice");
        }

        private void HandleTenantAdded(OneUnitTestEventOccurred testEvent)
        {
            Console.WriteLine(FormattableString.Invariant($"One unit test event #{testEvent.Id} occured - event received."));
            _oneCounter++;
        }

        private void HandleTenantDeleted(AnotherUnitTestEventOccurred testEvent)
        {
            Console.WriteLine(FormattableString.Invariant($"Another unit test event #{testEvent.Id} occured - event received."));
            _anotherCounter++;
        }
    }
}
