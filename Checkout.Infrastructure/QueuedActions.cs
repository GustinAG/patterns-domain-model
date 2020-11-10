using System;
using System.Collections.Generic;
using System.Linq;
using Checkout.Domain;

namespace Checkout.Infrastructure
{
    internal sealed class QueuedActions
    {
        private readonly Dictionary<DomainEvent, IList<Action>> _actionGroups = new Dictionary<DomainEvent, IList<Action>>();

        internal void Add(DomainEvent domainEvent) => _actionGroups.Add(domainEvent, new List<Action>());

        internal void Add(DomainEvent domainEvent, Action action)
        {
            if (!_actionGroups.ContainsKey(domainEvent)) return;

            _actionGroups[domainEvent].Add(action);
        }

        internal void Clear() => _actionGroups.Clear();

        internal void PlayAll() => _actionGroups.OrderBy(a => a.Key.Id).SelectMany(a => a.Value).ToList().ForEach(a => a());
    }
}
