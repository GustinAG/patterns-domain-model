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

        public static Customer Unknown { get; } = new Customer(BirthDate.Unknown);

        public bool IsAdult => this != Unknown && _birthDate.CurrentAge >= 18;

        protected override IList<object> EqualityComponents => new List<object> { _birthDate };
    }
}