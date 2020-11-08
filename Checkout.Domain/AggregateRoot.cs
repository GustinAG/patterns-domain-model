namespace Checkout.Domain
{
    /// <summary>
    /// Base class of each aggregate root value object.
    /// <para>
    /// Repositories always work with aggregate roots! One repository works with one aggregate root.
    /// </para>
    /// <para>
    /// In ideal case, only the aggregate roots are public - whereas all other value objects are internal within the Domain Model.
    /// </para>
    /// </summary>
    public abstract class AggregateRoot : ValueObject
    { }
}
