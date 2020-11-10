using System.Diagnostics.CodeAnalysis;

namespace Checkout.Domain
{
    // See also: https://msdn.microsoft.com/en-us/magazine/dn857357.aspx
    // http://udidahan.com/2009/06/14/domain-events-salvation
    // http://www.wrox.com/go/domaindrivendesign --> Chapter 18: Domain Events
    public interface IDomainEventCollector
    {
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This method does not work with the .NET Framework event model.")]
        void Raise<T>(T domainEvent) where T : DomainEvent;
    }
}
