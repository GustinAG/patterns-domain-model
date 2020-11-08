using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dawn;

namespace Checkout.Domain
{
    public static class HashCodeHelper
    {
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Guard validates it here.")]
        public static int CombineHashCodes(IList<object> objects)
        {
            Guard.Argument(objects, nameof(objects)).NotNull();

            unchecked
            {
                return objects.Aggregate(17, (h, o) => h * 23 + (o?.GetHashCode() ?? 0));
            }
        }
    }
}
