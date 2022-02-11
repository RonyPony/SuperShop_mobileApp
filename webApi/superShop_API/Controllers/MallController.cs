using Microsoft.AspNetCore.Authorization;
using superShop_API.Controllers.Base;
using superShop_API.Database.DTOs;
using superShop_API.Database.Entities;
using superShop_API.Database.Services;
using superShop_API.Database.Services.Constructor;

namespace superShop_API.Controllers;

[Authorize(Roles = Roles.Admin)]
public class MallController : BaseController<MallService, MallDto, Mall>
{
    public MallController(IServiceConstructor _constructor) : base(_constructor)
    {
    }
}