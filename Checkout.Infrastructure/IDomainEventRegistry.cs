using System;
using Checkout.Domain;

namespace Checkout.Infrastructure
{
    public interface IDomainEventRegistry
    {
        void Register<T>(Action<T> handler) where T : DomainEvent;
        void PlayAll();
    }
}