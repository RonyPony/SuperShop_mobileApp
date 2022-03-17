using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class ProductOrderDto : BaseDto<ProductOrder, Guid>
{
    public Guid ProductId { get; set; }

    public ProductDto Product { get; set; }

    public Guid OrderId { get; set; }

    public OrderDto Order { get; set; }
    public ProductOrderDto()
    {
    }

    public ProductOrderDto(ProductOrder entity) : base(entity)
    {
        ProductId = entity.ProductId;
        Product = new ProductDto(entity.Product);
        OrderId = entity.OrderId;
        Order = new OrderDto(entity.Order);
    }

    protected override ProductOrder MakeEntity()
    {
        return new ProductOrder
        {
            OrderId = OrderId,
            ProductId = ProductId
        };
    }
}