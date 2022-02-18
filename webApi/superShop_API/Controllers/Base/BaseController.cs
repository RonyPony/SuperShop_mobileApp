using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
/// <typeparam name="Tservice">Interfaz implementadora del servicio a utilizar para este controlador</typeparam>
/// <typeparam name="Tview">Tipo de vista a utilizar en este controlador</typeparam>
/// <typeparam name="Tentity">Tipo de modelo en el cual se ha creado el Tservice</typeparam>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class BaseController<Tservice, Tview, Tentity> : ControllerBase where Tentity : class, IBaseEntity, ISeeder<Tentity> where Tview : BaseDto<Tentity> where Tservice : class, IBaseService<Tentity>
{
    protected readonly IServiceConstructor Constructor;
    protected readonly Tservice Service;
    /// <summary>
    /// Constructor generico del controlador
    /// </summary>
    /// <param name="_constructor">Instancia de constructor de servicios</param>
    protected BaseController(IServiceConstructor _constructor)
    {
        Constructor = _constructor;
        Service = Constructor.GetService<Tservice, Tentity>();
    }

    /// <summary>
    /// Obtiene todas las entidades del modelo solicitante
    /// </summary>
    /// <returns>Lista de las vistas de los modelos resultantes</returns>
    [HttpGet]
    [Route("All", Name = "GetAll[controller]")]
    public virtual async Task<ActionResult<IList<Tview>>> GetAllAsync() => (await Service.GetAllAsync()).ConvertAll(m => (Tview)Activator.CreateInstance(typeof(Tview), m));

    /// <summary>
    /// Obtiene los detalles de una entidad de modelo en base a su identificador en base de datos
    /// </summary>
    /// <param name="id">Identificador unico de la entidad a consultar</param>
    /// <returns>Vista de la entidad de modelo resultante</returns>
    [HttpGet]
    [Route("Id", Name = "Get[controller]ByID")]
    public virtual async Task<ActionResult<Tview>> GetByIDAsync(Guid id) => (Tview)Activator.CreateInstance(typeof(Tview), await Service.GetByIDAsync(id));

    /// <summary>
    /// Actualiza una entidad de modelo o en su defecto crea el mismo en el sistema
    /// </summary>
    /// <param name="view">Vista de la entidad de modelo a evaluar</param>
    /// <returns>Resultado de la operacion de ingreso/actualizacion de datos</returns>
    [HttpPost]
    [Route("save", Name = "PostSave[controller]")]
    public async virtual Task<ActionResult<Result>> PostSaveChangesAsync([FromBody] /*JObject*/ Tview view)
    {
        Result Result = Result.Instance().Fail($"La vista recibida no es de tipo '{typeof(Tview).Name}' ");
        // if (view.ToObject<Tview>() is Tview cview)
        // {
        if (view.Id == Guid.Empty)
        {
            Result = await Service.CreateAsync(view.Entity);
        }
        // }
        return Result;
    }

    [HttpPut]
    [Route("update", Name = "PutUpdate[controller]")]
    public virtual async Task<ActionResult<Result>> PutUpdateChangesAsync([FromBody] /*JObject*/ Tview view)
    {
        Result Result = Result.Instance().Fail($"La vista recibida no es de tipo '{typeof(Tview).Name}' ");
        // if (view.ToObject<Tview>() is Tview cview)
        // {
        if (view.Id != Guid.Empty)
        {
            Result = await Service.UpdateAsync(view.Entity);
        }
        // }
        return Result;
    }

    /// <summary>
    /// Eliminar los detalles de una entidad de modelo en el sistema
    /// </summary>
    /// <param name="ID">Identificador unico del modelo a eliminar</param>
    /// <returns>Resultado de la operacion de eliminacion de datos</returns>
    [HttpDelete]
    [Route("remove", Name = "Delete[controller]")]
    public async virtual Task<ActionResult<Result>> DeleteRemoveChangesAsync([FromRoute] Guid Id) => await Service.DeleteAsync(Id);
}