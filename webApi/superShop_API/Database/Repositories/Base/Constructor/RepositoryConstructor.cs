using System.Reflection;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Repositories.Base;

namespace superShop_API.Database.Repositories.Constructor;

public interface IRepositoryConstructor : IDisposable, IAsyncDisposable
{
    IBaseRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class, IBaseEntity<TKey> where TKey : IEquatable<TKey>;
    TRepository GetRepositoryImplementation<TRepository, TEntity, TKey>() where TRepository : IBaseRepository<TEntity, TKey> where TEntity : class, IBaseEntity<TKey> where TKey : IEquatable<TKey>;
}

public class RepositoryConstructor : IRepositoryConstructor
{
    private readonly DatabaseContext Context;
    public RepositoryConstructor(DatabaseContext context)
    {
        Context = context;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Context.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return Context.DisposeAsync();
    }

    IBaseRepository<TEntity, TKey> IRepositoryConstructor.GetRepository<TEntity, TKey>() => new BaseRepository<TEntity, TKey>(Context);

    TRepository IRepositoryConstructor.GetRepositoryImplementation<TRepository, TEntity, TKey>()
    {
        Type? tConcreteRepository = null;
        Assembly.GetExecutingAssembly().GetTypes().ToList().ForEach(t =>
        {
            if (typeof(TRepository).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            {
                tConcreteRepository = t;
            }
        });
        return (TRepository)Activator.CreateInstance(tConcreteRepository, Context);
    }
}