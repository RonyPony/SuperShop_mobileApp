using Microsoft.AspNetCore.Authorization;
using superShop_API.Controllers.Base;
using superShop_API.Database.DTOs;
using superShop_API.Database.Entities;
using superShop_API.Database.Services;
using superShop_API.Database.Services.Constructor;

namespace superShop_API.Controllers;

[AllowAnonymous]
public class CategoryController : BaseController<CategoryService, CategoryDto, Category>
{
    public CategoryController(IServiceConstructor _constructor) : base(_constructor)
    {

    }
}