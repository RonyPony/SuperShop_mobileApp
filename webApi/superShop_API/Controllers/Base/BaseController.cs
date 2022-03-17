using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;
using superShop_API.Database.Services.Base;
using superShop_API.Database.Services.Constructor;
using superShop_API.Shared;

namespace superShop_API.Controllers.Base;

/// <summary>
/// Controlador generico para los controllers del proyecto
/// </summary>
/// <typeparam name="TService">Interfaz implementadora del servicio a utilizar para este controlador</typeparam>
/// <typeparam name="TView">Tipo de vista a utilizar en este controlador</typeparam>
/// <typeparam name="TEntity">Tipo de modelo en el cual se ha creado el TService</typeparam>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class BaseController<TService, TView, TEntity, TKey> : ControllerBase where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey> where TView : BaseDto<TEntity, TKey> where TService : class, IBaseService<TEntity, TKey> where TKey : IEquatable<TKey>
{
    protected readonly IServiceConstructor Constructor;
    protected readonly TService Service;
    /// <summary>
    /// Constructor generico del controlador
    /// </summary>
    /// <param name="_constructor">Instancia de constructor de servicios</param>
    protected BaseController(IServiceConstructor _constructor)
    {
        Constructor = _constructor;
        Service = Constructor.GetService<TService, TEntity, TKey>();
    }

    /// <summary>
    /// Obtiene todas las entidades del modelo solicitante
    /// </summary>
    /// <returns>Lista de las vistas de los modelos resultantes</returns>
    [HttpGet]
    [Route("All", Name = "GetAll[controller]")]
    public virtual async Task<ActionResult<IList<TView>>> GetAllAsync() => (await Service.GetAllAsync()).ConvertAll(m => (TView)Activator.CreateInstance(typeof(TView), m));

    /// <summary>
    /// Obtiene los detalles de una entidad de modelo en base a su identificador en base de datos
    /// </summary>
    /// <param name="id">Identificador unico de la entidad a consultar</param>
    /// <returns>Vista de la entidad de modelo resultante</returns>
    [HttpGet]
    [Route("{Id}", Name = "Get[controller]ByID")]
    public virtual async Task<ActionResult<TView>> GetByIDAsync([FromRoute] TKey Id) => (TView)Activator.CreateInstance(typeof(TView), await Service.GetByIDAsync(Id));

    /// <summary>
    /// Actualiza una entidad de modelo o en su defecto crea el mismo en el sistema
    /// </summary>
    /// <param name="view">Vista de la entidad de modelo a evaluar</param>
    /// <returns>Resultado de la operacion de ingreso/actualizacion de datos</returns>
    [HttpPost]
    [Route("save", Name = "PostSave[controller]")]
    public async virtual Task<ActionResult<Result<Object>>> PostSaveChangesAsync([FromBody] /*JObject*/ TView view)
    {
        try
        {
            var R = Result.Instance().Fail($"La vista recibida no es de tipo '{typeof(TView).Name}' ");
            if (view.Id == null)
            {
                R = await Service.CreateAsync(view.Entity);
            }

            if (R.IsSuccess)
            {
                return R;
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, R);
            }

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("These occurred an error on this request", exception: e));
        }
    }

    [HttpPut]
    [Route("update", Name = "PutUpdate[controller]")]
    public virtual async Task<ActionResult<Result<Object>>> PutUpdateChangesAsync([FromBody] /*JObject*/ TView view)
    {
        try
        {
            var R = Result.Instance().Fail($"La vista recibida no es de tipo '{typeof(TView).Name}' ");
            if (view.Id != null)
            {
                R = await Service.UpdateAsync(view.Entity);
            }

            if (R.IsSuccess)
            {
                return R;
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, R);
            }

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("These occurred an error on this request", exception: e));
        }
    }

    /// <summary>
    /// Eliminar los detalles de una entidad de modelo en el sistema
    /// </summary>
    /// <param name="ID">Identificador unico del modelo a eliminar</param>
    /// <returns>Resultado de la operacion de eliminacion de datos</returns>
    [HttpDelete]
    [Route("remove/{Id}", Name = "Delete[controller]")]
    public async virtual Task<ActionResult<Result<Object>>> DeleteRemoveChangesAsync([FromRoute] TKey Id) => await Service.DeleteAsync(Id);
}

/// <summary>
/// Controlador generico para los controllers del proyecto
/// </summary>
/// <typeparam name="TService">Interfaz implementadora del servicio a utilizar para este controlador</typeparam>
/// <typeparam name="TView">Tipo de vista a utilizar en este controlador</typeparam>
/// <typeparam name="TEntity">Tipo de modelo en el cual se ha creado el TService</typeparam>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class BaseController<TService, TView, TEntity, TKey, T> : ControllerBase where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey, T> where TView : BaseDto<TEntity, TKey, T> where TService : class, IBaseService<TEntity, TKey, T> where TKey : IEquatable<TKey>
{
    protected readonly IServiceConstructor Constructor;
    protected readonly TService Service;
    /// <summary>
    /// Constructor generico del controlador
    /// </summary>
    /// <param name="_constructor">Instancia de constructor de servicios</param>
    protected BaseController(IServiceConstructor _constructor)
    {
        Constructor = _constructor;
        Service = Constructor.GetService<TService, TEntity, TKey, T>();
    }

    /// <summary>
    /// Obtiene todas las entidades del modelo solicitante
    /// </summary>
    /// <returns>Lista de las vistas de los modelos resultantes</returns>
    [HttpGet]
    [Route("All", Name = "GetAll[controller]")]
    public virtual async Task<ActionResult<IList<TView>>> GetAllAsync() => (await Service.GetAllAsync()).ConvertAll(m => (TView)Activator.CreateInstance(typeof(TView), m));

    /// <summary>
    /// Obtiene los detalles de una entidad de modelo en base a su identificador en base de datos
    /// </summary>
    /// <param name="id">Identificador unico de la entidad a consultar</param>
    /// <returns>Vista de la entidad de modelo resultante</returns>
    [HttpGet]
    [Route("{Id}", Name = "Get[controller]ByID")]
    public virtual async Task<ActionResult<TView>> GetByIDAsync([FromRoute(Name = "Id")] TKey id) => (TView)Activator.CreateInstance(typeof(TView), await Service.GetByIDAsync(id));

    /// <summary>
    /// Actualiza una entidad de modelo o en su defecto crea el mismo en el sistema
    /// </summary>
    /// <param name="view">Vista de la entidad de modelo a evaluar</param>
    /// <returns>Resultado de la operacion de ingreso/actualizacion de datos</returns>
    [HttpPost]
    [Route("save", Name = "PostSave[controller]")]
    public async virtual Task<ActionResult<Result<Object>>> PostSaveChangesAsync([FromBody] /*JObject*/ TView view)
    {
        var R = Result.Instance().Fail($"La vista recibida no es de tipo '{typeof(TView).Name}' ");
        if (view.Id == null)
        {
            R = await Service.CreateAsync(view.Entity);
        }
        return R;
    }

    [HttpPut]
    [Route("update", Name = "PutUpdate[controller]")]
    public virtual async Task<ActionResult<Result<Object>>> PutUpdateChangesAsync([FromBody] /*JObject*/ TView view)
    {
        var R = Result.Instance().Fail($"La vista recibida no es de tipo '{typeof(TView).Name}' ");
        if (view.Id != null)
        {
            R = await Service.UpdateAsync(view.Entity);
        }
        return R;
    }

    /// <summary>
    /// Eliminar los detalles de una entidad de modelo en el sistema
    /// </summary>
    /// <param name="Id">Identificador unico del modelo a eliminar</param>
    /// <returns>Resultado de la operacion de eliminacion de datos</returns>
    [HttpDelete]
    [Route("remove/{Id}", Name = "Delete[controller]")]
    public async virtual Task<ActionResult<Result<Object>>> DeleteRemoveChangesAsync([FromRoute(Name = "Id")] TKey Id) => await Service.DeleteAsync(Id);
}
