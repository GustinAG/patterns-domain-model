using System;
using System.Collections.Generic;

namespace Checkout.Domain.Checkout
{
    public sealed class Customer : ValueObject
    {
        private readonly BirthDate _birthDate;

        internal Customer(BirthDate birthDate)
        {
            _birthDate = birthDate;
        }

        protected override IList<object> EqualityComponents => new List<object> { _birthDate };
    }
}