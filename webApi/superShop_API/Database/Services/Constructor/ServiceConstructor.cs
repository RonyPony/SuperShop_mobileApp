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
    /// <typeparam name="Tservice">interface del servicio que implementa IServicioBase<Tmodel> del cual se desea obtener una nueva instancia del mismo</typeparam>
    /// <typeparam name="Tentity">modelo del cual se ha creado el servicio a inicializar</typeparam>
    /// <returns>Retorna el servicio del cual se ha hecho solicitud de una nueva instancia</returns>
    Tservice GetService<Tservice, Tentity>() where Tservice : IBaseService<Tentity> where Tentity : class, IBaseEntity, ISeeder<Tentity>;
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

    Tservice IServiceConstructor.GetService<Tservice, Tmodel>()
    {
        Type? tConcreteService = null;
        Assembly.GetExecutingAssembly().GetTypes().ToList().ForEach(t =>
        {
            if (typeof(Tservice).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            {
                tConcreteService = t;
            }
        });
        return (Tservice)Activator.CreateInstance(tConcreteService, Constructor);
    }
}