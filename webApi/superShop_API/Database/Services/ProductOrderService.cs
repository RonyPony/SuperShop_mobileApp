using superShop_API.Database.Entities;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public interface IProductOrderService : IBaseService<ProductOrder, Guid>
{
    Task<List<ProductOrder>> GetByOrderId(Guid orderId);
    Task<List<ProductOrder>> GetByProductId(Guid productId);
}

public class ProductOrderService : BaseService<ProductOrder, Guid>, IProductOrderService
{
    public ProductOrderService(IRepositoryConstructor constructor) : base(constructor)
    {
    }

    public async Task<List<ProductOrder>> GetByOrderId(Guid orderId)
    {
        return (await this.Repository.GetAsync(po => po.OrderId == orderId)).ToList();
    }

    public async Task<List<ProductOrder>> GetByProductId(Guid productId)
    {
        return (await this.Repository.GetAsync(po => po.ProductId == productId)).ToList();
    }

    public async override Task<Result<Object>> ValidateOnCreateAsync(ProductOrder entity)
    {
        return await Task.Run(() => Result.Instance().Success("Valid !"));
    }
}