using System;
using System.Collections.Generic;
using System.Linq;
using Checkout.Domain;

namespace Checkout.Infrastructure
{
    // Based on: https://msdn.microsoft.com/en-us/magazine/dn857357.aspx
    // See also: http://udidahan.com/2009/06/14/domain-events-salvation/
    public sealed class DomainEvents : IDomainEventCollector, IDomainEventRegistry
    {
        private readonly List<Delegate> _handlers = new List<Delegate>();
        private readonly QueuedActions _queuedActions = new QueuedActions();

        public void Register<T>(Action<T> handler) where T : DomainEvent => _handlers.Add(handler);

        public void Raise<T>(T domainEvent) where T : DomainEvent
        {
            _queuedActions.Add(domainEvent);
            _handlers.Where(h => h is Action<T>).Cast<Action<T>>().ToList().ForEach(a => _queuedActions.Add(domainEvent, () => a(domainEvent)));
        }

        public void PlayAll()
        {
            _queuedActions.PlayAll();
            _queuedActions.Clear();
        }
    }
}
