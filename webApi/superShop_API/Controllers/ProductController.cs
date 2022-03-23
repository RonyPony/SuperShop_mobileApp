using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using superShop_API.Controllers.Base;
using superShop_API.Database.DTOs;
using superShop_API.Database.Entities;
using superShop_API.Database.Services;
using superShop_API.Database.Services.Constructor;
using superShop_API.Shared;

namespace superShop_API.Controllers;

[AllowAnonymous]
//[Authorize(Roles = Roles.Admin)]
public class ProductController : BaseController<ProductService, ProductDto, Product, Guid>
{
    public ProductController(IServiceConstructor _constructor) : base(_constructor)
    {
    }

    //public async override Task<ActionResult<Result<Object>>> DeleteRemoveChangesAsync([FromRoute(Name = "Id")] string Id) => await Service.DeleteAsync(Guid.Parse(Id));

    [HttpGet]
    [Authorize(Roles = Roles.User)]
    [Route("by-branch/{branchId}")]
    public async Task<ActionResult<IList<ProductDto>>> GetAllByBranchId(Guid branchId) => (await this.Service.GetAllByBranchId(branchId)).ConvertAll(p => new ProductDto(p));
}