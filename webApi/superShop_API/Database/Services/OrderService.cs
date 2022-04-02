using superShop_API.Database.Entities;
using superShop_API.Database.Entities.Auth;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Database.Services.Constructor;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public interface IOrderService : IBaseService<Order, Guid, OrderSeedParams>
{
    Task<List<Order>> GetOrdersByUser(Guid userId, bool completed = false);
}

public class OrderService : BaseService<Order, Guid, OrderSeedParams>, IOrderService
{
    private ProductOrderService poService { get; set; }
    public OrderService(IRepositoryConstructor constructor) : base(constructor)
    {
        poService = new ServiceConstructor(Constructor).GetService<ProductOrderService, ProductOrder, Guid>();
    }

    public async Task<List<Order>> GetOrdersByUser(Guid userId, bool completed = false)
    {
        return (await Repository.GetAsync(o => o.UserId == userId && o.Completed == completed)).ToList();
    }

    public override async Task<List<Order>> GetAllAsync()
    {
        var orders = (await this.Repository.GetAllAsync()).ToList();
        var orderList = new List<Order>();
        foreach (var o in orders)
        {
            var poList = await poService.GetByOrderId(o.Id);

            if (poList.Count() > 0)
            {
                o.Products = new List<Product>();

                foreach (var p in poList)
                {
                    var product = await this.Constructor.GetRepository<Product, Guid>().GetByIDAsync(p.ProductId);
                    o.Products.Add(product);
                }

                orderList.Add(o);
            }
            else
            {
                o.Products = null;
                o.ProductOrders = null;
                orderList.Add(o);
            }
        }

        return orderList;
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
            return Result.Instance().Fail("These an error while validating the new order to save", e);
        }
    }

    public async Task<Result<Object>> CreateNewOrder((Guid UserId, Guid BranchId, string Address, bool Completed, Guid[] ProductIds, TransactionDetails? TransactionDetails) details)
    {
        try
        {
            var R = Result.Instance();
            var user = await this.Constructor.GetRepository<User, Guid>().GetByIDAsync(details.UserId);
            var branch = await this.Constructor.GetRepository<Branch, Guid>().GetByIDAsync(details.BranchId);

            if (user != null && branch != null)
            {

                var productList = new List<Product>();

                foreach (var id in details.ProductIds)
                {
                    var product = await this.Constructor.GetRepository<Product, Guid>().GetByIDAsync(id);

                    if (product != null)
                    {
                        productList.Add(product);
                    }
                    else
                    {
                        throw new Exception($"The product whit id '{id}' cannot be found. Correct this and try again !");
                    }
                }

                var order = new Order
                {
                    UserId = details.UserId,
                    BranchId = details.BranchId,
                    Address = details.Address,
                    Completed = details.Completed,
                    TransactionDetails = details.TransactionDetails
                };

                foreach (var p in productList)
                {
                    var TotalWhitoutTaxes = Convert.ToDouble(p.Price);
                    var TotalTax = Convert.ToDouble(TotalWhitoutTaxes * 0.18);

                    order.TotalWhitoutTaxes = TotalWhitoutTaxes + order.TotalWhitoutTaxes;
                    order.TotalTax = TotalTax + order.TotalTax;
                    order.Total = TotalWhitoutTaxes + TotalTax + order.Total;
                }

                R = await this.CreateAsync(order);

                if (R.IsSuccess)
                {
                    var poR = await this.poService.CreateRangeAsync(productList.ConvertAll(p => new ProductOrder { ProductId = p.Id, OrderId = order.Id }));

                    if (!poR.IsSuccess)
                    {
                        R = Result.Instance().Fail($"The product orders relations cannot be saved !");
                    }
                }

            }
            else if (user == null)
            {
                R = Result.Instance().Fail($"The user whit the ID '{details.UserId}' doesn't exists !");
            }
            else if (branch == null)
            {
                R = Result.Instance().Fail($"The branch whit the ID '{details.BranchId}' doesn't exists !");
            }
            return R;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("These have an error while saving the new requested order", e);
        }
    }

    public override async Task<Result<Object>> DeleteAsync(Guid id)
    {
        try
        {
            var r = await poService.DeleteRangeAsync(await poService.GetByOrderId(id));
            if (r.IsSuccess)
            {
                r = await base.DeleteAsync(id);
            }
            return r;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", e);
        }
    }
}