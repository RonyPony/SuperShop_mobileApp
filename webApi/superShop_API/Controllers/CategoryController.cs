using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using superShop_API.Controllers.Base;
using superShop_API.Database.DTOs;
using superShop_API.Database.Entities;
using superShop_API.Database.Services;
using superShop_API.Database.Services.Constructor;

namespace superShop_API.Controllers;

[AllowAnonymous]
[DisableCors]
public class CategoryController : BaseController<CategoryService, CategoryDto, Category, Guid>
{
    public CategoryController(IServiceConstructor _constructor) : base(_constructor)
    {

    }
}