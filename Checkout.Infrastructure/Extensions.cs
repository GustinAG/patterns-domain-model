using System;
using System.Collections.Generic;
using Dawn;

namespace Checkout.Infrastructure
{
    internal static class Extensions
    {
        internal static void ForEach<T>(this IList<T> items, Action<T> action)
        {
            Guard.Argument(items, nameof(items)).NotNull();
            Guard.Argument(action, nameof(action)).NotNull();

            foreach (var item in items) action(item);
        }
    }
}
