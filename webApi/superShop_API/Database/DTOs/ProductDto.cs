using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class ProductDto : BaseDto<Product>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string ImageUrl { get; set; }
    public Guid BranchId { get; set; }

    public ProductDto()
    {
    }

    public ProductDto(Product entity) : base(entity)
    {
        Name = entity.Name;
        Code = entity.Code;
        Description = entity.Description;
        Price = entity.Price;
        Stock = entity.Stock;
        ImageUrl = entity.ImageUrl;
        BranchId = entity.BranchId;
    }

    protected override Product MakeEntity()
    {
        return new Product
        {
            Name = Name,
            Code = Code,
            Description = Description,
            Price = Price,
            Stock = Stock,
            ImageUrl = ImageUrl,
            BranchId = BranchId
        };
    }
}