using superShop_API.Database.Entities;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public class OrderService : BaseService<Order, Guid, OrderSeedParams>
{
    public OrderService(IRepositoryConstructor constructor) : base(constructor)
    {
    }

    public async override Task<Result<Object>> ValidateOnCreateAsync(Order entity)
    {
        try
        {
            var orderFound = (await this.Repository.GetAsync(o => o.BranchId == entity.BranchId && o.UserId == entity.UserId && o.Completed == false)).FirstOrDefault();
            if (orderFound == null)
            {
                return Result.Instance().Success("The requested order is valid to be created !");
            }
            else
            {
                return Result.Instance().Fail("The requested order to create has another order whitout complete !. Close the order and try again.");
            }
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("", e);
        }
    }
}