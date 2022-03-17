using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using superShop_API.Controllers.Base;
using superShop_API.Database.DTOs;
using superShop_API.Database.Entities;
using superShop_API.Database.Services;
using superShop_API.Database.Services.Constructor;

namespace superShop_API.Controllers;

[AllowAnonymous]
[DisableCors]
//[Authorize(Roles = Roles.Admin)]
public class BranchController : BaseController<BranchService, BranchDto, Branch, Guid, BranchSeedParams>
{
    public BranchController(IServiceConstructor _constructor) : base(_constructor)
    {
    }

    [HttpGet]
    [Authorize(Roles = Roles.User)]
    [Route("by-mall/{mallId}")]
    public async Task<ActionResult<IList<BranchDto>>> GetAllByMallId([FromRoute] Guid mallId) => (await this.Service.GetAllByMallId(mallId)).ConvertAll(b => new BranchDto(b));

    [HttpGet]
    [Authorize(Roles = Roles.User)]
    [Route("by-category/{categoryId}")]
    public async Task<ActionResult<IList<BranchDto>>> GetAllByCategoryId([FromRoute] Guid categoryId) => (await this.Service.GetAllByCategoryId(categoryId)).ConvertAll(b => new BranchDto(b));
}