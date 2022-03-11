using System.ComponentModel.DataAnnotations;
using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class ProductDto : BaseDto<Product, ProductSeedParams>
{
    [Required(ErrorMessage = "A name of this product is required")]
    [MaxLength(80, ErrorMessage = "The name of a product must be at least 80 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "An unique code is required for this product")]
    [MaxLength(32, ErrorMessage = "The code of a product must be at least 32 characters")]
    public string Code { get; set; }

    [Required(ErrorMessage = "a description for this product is required")]
    [MaxLength(256, ErrorMessage = "The description of a product must be at least 256 characters")]
    public string Description { get; set; }

    [Required(ErrorMessage = "The price of a product is required")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "the amount of products in stock is required")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "The image url for this product is required")]
    public string ImageUrl { get; set; }

    [Required(ErrorMessage = "The category of this product is required")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "The owner branch of this product is required")]
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
        CategoryId = entity.CategoryId;
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
            CategoryId = CategoryId,
            BranchId = BranchId
        };
    }
}