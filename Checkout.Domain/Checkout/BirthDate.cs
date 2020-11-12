using System;
using System.Collections.Generic;

namespace Checkout.Domain.Checkout
{
    public sealed class BirthDate : ValueObject
    {
        private readonly DateTime _date;

        public BirthDate(DateTime date)
        {
            _date = date;
        }

        public static BirthDate Unknown { get; } = new BirthDate(DateTime.MinValue);

        public int CurrentAge => _date.GetAge();

        protected override IList<object> EqualityComponents => new List<object> { _date };
    }
}