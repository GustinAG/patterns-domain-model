namespace DomainModel.AppService
{
    /// <summary>
    /// Dependency resolver.
    /// </summary>
    public interface IResolver
    {
        T Resolve<T>();
    }
}
