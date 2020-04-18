using System.Collections.Generic;
using Dawn;

namespace DomainModel.Domain
{
    public static class HashCodeHelper
    {
        public static int CombineHashCodes(IList<object> objects)
        {
            Guard.Argument(objects, nameof(objects)).NotNull();

            unchecked
            {
                var hash = 17;

                foreach (var obj in objects) hash = hash * 23 + (obj?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
