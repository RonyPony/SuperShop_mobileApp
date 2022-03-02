using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using superShop_API.Controllers.Base;
using superShop_API.Database.DTOs;
using superShop_API.Database.Entities;
using superShop_API.Database.Services;
using superShop_API.Database.Services.Constructor;

namespace superShop_API.Controllers;

[Authorize(Roles = Roles.Admin)]
public class ProductController : BaseController<ProductService, ProductDto, Product>
{
    public ProductController(IServiceConstructor _constructor) : base(_constructor)
    {
    }

    [HttpGet]
    [Authorize(Roles = Roles.User)]
    [Route("All", Name = "GetAllProducts")]
    public override async Task<ActionResult<IList<ProductDto>>> GetAllAsync() => await base.GetAllAsync();


    [HttpGet]
    [Authorize(Roles = Roles.User)]
    [Route("Id", Name = "GetProductsByID")]
    public override async Task<ActionResult<ProductDto>> GetByIDAsync(Guid id) => await base.GetByIDAsync(id);
}