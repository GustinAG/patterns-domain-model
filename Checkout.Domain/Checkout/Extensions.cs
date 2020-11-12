using System;

namespace Checkout.Domain.Checkout
{
    internal static class Extensions
    {
        // Based on: https://stackoverflow.com/questions/9/in-c-how-do-i-calculate-someones-age-based-on-a-datetime-type-birthday
        internal static int GetAge(this DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
