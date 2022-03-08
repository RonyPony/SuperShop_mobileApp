using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class OrderDto : BaseDto<Order, OrderSeedParams>
{
    public OrderDto()
    {
    }

    public OrderDto(Order entity) : base(entity)
    {
        BranchId = entity.BranchId;
        UserId = entity.UserId;
        Address = entity.Address;
        Total = entity.Total;
        Completed = entity.Completed;
        Branch = new BranchDto(entity.Branch);
        ProductOrderDtos = entity.ProductOrders != null ? entity.ProductOrders.ToList().ConvertAll(po => new ProductOrderDto(po)) : new List<ProductOrderDto>();
    }

    public Guid BranchId { get; set; }

    public Guid UserId { get; set; }

    public string Address { get; set; }

    public double Total { get; set; }

    public bool Completed { get; set; }

    public BranchDto Branch { get; set; }

    public IList<ProductOrderDto>? ProductOrderDtos { get; set; }
    protected override Order MakeEntity()
    {
        return new Order
        {
            BranchId = BranchId,
            UserId = UserId,
            Address = Address,
            Total = Total,
            Completed = Completed,
            ProductOrders = ProductOrderDtos.ToList().ConvertAll(po => po.Entity)
        };
    }
}