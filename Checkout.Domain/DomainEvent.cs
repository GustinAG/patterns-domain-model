namespace Checkout.Domain
{
    // See also: https://msdn.microsoft.com/en-us/magazine/dn857357.aspx
    // http://udidahan.com/2009/06/14/domain-events-salvation
    // http://www.wrox.com/go/domaindrivendesign --> Chapter 18: Domain Events
    public abstract class DomainEvent
    {
        private static int _lastId;

        protected DomainEvent()
        {
            Id = ++_lastId;
        }

        public int Id { get; }
    }
}
