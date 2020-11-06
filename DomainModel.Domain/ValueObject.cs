using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainModel.Domain
{
    /// <summary>
    /// Please make all inheritors immutable!
    /// <para>
    /// In ideal case, only the aggregate roots are public - whereas all other value objects are internal within the Domain Model.
    /// </para>
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        protected abstract IList<object> EqualityComponents { get; }

        #region Equality
        public static bool operator ==(ValueObject valueObject1, ValueObject valueObject2) =>
            valueObject1 is null || valueObject2 is null ? Equals(valueObject1, valueObject2) : valueObject1.Equals(valueObject2);

        public static bool operator !=(ValueObject valueObject1, ValueObject valueObject2) =>
            valueObject1 is null || valueObject2 is null ? !Equals(valueObject1, valueObject2) : !valueObject1.Equals(valueObject2);

        public bool Equals(ValueObject other) => other != null && EqualityComponents.SequenceEqual(other.EqualityComponents);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null) return false;
            if (GetType() != obj.GetType()) return false;

            var valueObject = obj as ValueObject;
            return valueObject != null && Equals(valueObject);
        }

        public override int GetHashCode() => HashCodeHelper.CombineHashCodes(EqualityComponents);
        #endregion

        // Only for debugging / tracing purposes:
        public override string ToString() => string.Join(" / ", EqualityComponents);
    }
}
