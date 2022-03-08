using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using superShop_API.Controllers.Base;
using superShop_API.Database.DTOs;
using superShop_API.Database.Entities;
using superShop_API.Database.Services;
using superShop_API.Database.Services.Constructor;

namespace superShop_API.Controllers;

[Authorize(Roles = Roles.Admin)]
public class BranchController : BaseController<BranchService, BranchDto, Branch>
{
    public BranchController(IServiceConstructor _constructor) : base(_constructor)
    {
    }

    [HttpGet]
    //[Authorize(Roles = Roles.User)]
    [AllowAnonymous]
    [Route("All", Name = "GetAllBranches")]
    public override async Task<ActionResult<IList<BranchDto>>> GetAllAsync() => await base.GetAllAsync();


    [HttpGet]
    //[Authorize(Roles = Roles.User)]
    [AllowAnonymous]
    [Route("Id", Name = "GetBranchsByID")]
    public override async Task<ActionResult<BranchDto>> GetByIDAsync(Guid id) => await base.GetByIDAsync(id);
}