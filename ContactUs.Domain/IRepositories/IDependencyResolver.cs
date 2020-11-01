namespace ConnectUs.Domain.IRepositories
{
    public interface IDependencyResolver
    {
        T ResolveOrDefault<T>() where T : class;

    }
}
