using System.Reflection;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Repositories.Base;

namespace superShop_API.Database.Repositories.Constructor;

public interface IRepositoryConstructor : IDisposable, IAsyncDisposable
{
    IBaseRepository<Tentity> GetRepository<Tentity>() where Tentity : class, IBaseEntity;
    TRepository GetRepositoryImplementation<TRepository, Tentity>() where TRepository : IBaseRepository<Tentity> where Tentity : class, IBaseEntity;
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

    public IBaseRepository<Tentity> GetRepository<Tentity>() where Tentity : class, IBaseEntity => new BaseRepository<Tentity>(Context);

    public TRepository GetRepositoryImplementation<TRepository, Tentity>() where TRepository : IBaseRepository<Tentity> where Tentity : class, IBaseEntity
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