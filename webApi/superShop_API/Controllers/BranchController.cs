using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using superShop_API.Controllers.Base;
using superShop_API.Database.DTOs;
using superShop_API.Database.Entities;
using superShop_API.Database.Services;
using superShop_API.Database.Services.Constructor;

namespace superShop_API.Controllers;

[AllowAnonymous]
//[Authorize(Roles = Roles.Admin)]
public class BranchController : BaseController<BranchService, BranchDto, Branch>
{
    public BranchController(IServiceConstructor _constructor) : base(_constructor)
    {
    }

    [HttpGet]
    [Authorize(Roles = Roles.User)]
    [Route("by-mall/{mallId}")]
    public async Task<ActionResult<IList<BranchDto>>> GetAllByMallId([FromRoute] Guid mallId) => (await this.Service.GetAllByMallId(mallId)).ConvertAll(b => new BranchDto(b));
}