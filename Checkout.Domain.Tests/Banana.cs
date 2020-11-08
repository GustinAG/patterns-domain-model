using System.Collections.Generic;

namespace Checkout.Domain.Tests
{
    internal class Banana : ValueObject
    {
        private readonly string _color;
        private readonly int _size;

        internal Banana(string color, int size)
        {
            _color = color;
            _size = size;
        }

        protected override IList<object> EqualityComponents => new List<object> { _color, _size };
    }
}
