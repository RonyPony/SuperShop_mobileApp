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
public class OrderController : BaseController<OrderService, OrderDto, Order, Guid, OrderSeedParams>
{
    public OrderController(IServiceConstructor _constructor) : base(_constructor)
    {
    }

    //public async override Task<ActionResult<Result<Object>>> DeleteRemoveChangesAsync([FromRoute(Name = "Id")] string Id) => await Service.DeleteAsync(Guid.Parse(Id));


    [HttpPost]
    [Route("new", Name = "PostSaveNewOrder")]
    public async virtual Task<ActionResult<Result<Object>>> PostSaveNewOrder([FromBody] NewOrderDto view)
    {
        try
        {
            var R = Result.Instance().Fail($"La vista recibida no es de tipo 'NewOrder' ");
            if (view.Id == Guid.Empty)
            {
                R = await Service.CreateNewOrder((view.UserId, view.BranchId, view.Address, view.Completed, view.ProductIds));
            }

            if (R.IsSuccess)
            {
                return R;
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, R);
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, Result.Instance().Fail("These occurred an error on this request", exception: e));
        }
    }
}