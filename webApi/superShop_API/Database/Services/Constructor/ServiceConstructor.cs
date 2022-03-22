using System.Reflection;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Seeders;
using superShop_API.Database.Services.Base;

namespace superShop_API.Database.Services.Constructor;

/// <summary>
/// interfaz para el manejo global de constructores de servicios de consultas en base de datos
/// </summary>
public interface IServiceConstructor : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Obtiene una instancia del servicio que se solicite
    /// </summary>
    /// <typeparam name="TService">interface del servicio que implementa IServicioBase<Tmodel> del cual se desea obtener una nueva instancia del mismo</typeparam>
    /// <typeparam name="TEntity">modelo del cual se ha creado el servicio a inicializar</typeparam>
    /// <returns>Retorna el servicio del cual se ha hecho solicitud de una nueva instancia</returns>
    TService GetService<TService, TEntity, TKey>() where TService : IBaseService<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey> where TKey : IEquatable<TKey>;

    /// <summary>
    /// Obtiene una instancia del servicio que se solicite
    /// </summary>
    /// <typeparam name="TService">interface del servicio que implementa IServicioBase<Tmodel> del cual se desea obtener una nueva instancia del mismo</typeparam>
    /// <typeparam name="TEntity">modelo del cual se ha creado el servicio a inicializar</typeparam>
    /// <typeparam name="T">tipo genérico de nuestro seeder customizados</typeparam>
    /// <returns>Retorna el servicio del cual se ha hecho solicitud de una nueva instancia</returns>
    TService GetService<TService, TEntity, TKey, T>() where TService : IBaseService<TEntity, TKey, T> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey, T> where TKey : IEquatable<TKey>;
}
public class ServiceConstructor : IServiceConstructor
{
    public IRepositoryConstructor Constructor { get; protected set; }
    /// <summary>
    /// Constructor de clase constructora de servicios
    /// </summary>
    /// <param name="_constructor">Instancia de la implementacion del constructor de instancias a repositorio de forma global global</param>
    public ServiceConstructor(IRepositoryConstructor _constructor)
    {
        Constructor = _constructor;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Constructor.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return Constructor.DisposeAsync();
    }

    public TService GetService<TService, TEntity, TKey>() where TService : IBaseService<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey> where TKey : IEquatable<TKey>
    {
        Type? tConcreteService = null;
        Assembly.GetExecutingAssembly().GetTypes().ToList().ForEach(t =>
        {
            if (typeof(TService).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            {
                tConcreteService = t;
            }
        });
        return (TService)Activator.CreateInstance(tConcreteService, Constructor);
    }

    public TService GetService<TService, TEntity, TKey, T>() where TService : IBaseService<TEntity, TKey, T> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey, T> where TKey : IEquatable<TKey>
    {
        Type? tConcreteService = null;
        Assembly.GetExecutingAssembly().GetTypes().ToList().ForEach(t =>
        {
            if (typeof(TService).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            {
                tConcreteService = t;
            }
        });
        return (TService)Activator.CreateInstance(tConcreteService, Constructor);
    }
}