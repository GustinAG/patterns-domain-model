using System;
using System.Collections.Generic;

namespace Checkout.Domain.Checkout
{
    public sealed class BirthDate : ValueObject
    {
        private readonly DateTime _date;

        internal BirthDate(DateTime date)
        {
            _date = date;
        }

        protected override IList<object> EqualityComponents => new List<object> { _date };
    }
}