using superShop_API.Database.Entities;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public class ProductOrderService : BaseService<ProductOrder, Guid>
{
    public ProductOrderService(IRepositoryConstructor constructor) : base(constructor)
    {
    }

    public async override Task<Result<Object>> ValidateOnCreateAsync(ProductOrder entity)
    {
        return await Task.Run(() => Result.Instance().Success("Valid !"));
    }
}