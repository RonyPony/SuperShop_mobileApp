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
public class ProductController : BaseController<ProductService, ProductDto, Product, Guid>
{
    public ProductController(IServiceConstructor _constructor) : base(_constructor)
    {
    }

    [HttpGet]
    [Authorize(Roles = Roles.User)]
    [Route("by-branch/{branchId}")]
    public async Task<ActionResult<IList<ProductDto>>> GetAllByBranchId(Guid branchId) => (await this.Service.GetAllByBranchId(branchId)).ConvertAll(p => new ProductDto(p));
}