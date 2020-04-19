using System;
using System.Collections.Generic;

namespace DomainModel.AppService
{
    /// <summary>
    /// An extremely simplified <see cref="IResolver"/> implementation - just for demo purposes.
    /// </summary>
    public class RegisteringResolver : IResolver
    {
        private readonly IDictionary<Type, Func<object>> _registry = new Dictionary<Type, Func<object>>();

        public void Register<T>(Func<object> instantiateFunc) => _registry.Add(typeof(T), instantiateFunc);

        public T Resolve<T>()
        {
            var interfaceType = typeof(T);
            return _registry.ContainsKey(interfaceType) ? (T)_registry[interfaceType]() : default;
        }
    }
}
