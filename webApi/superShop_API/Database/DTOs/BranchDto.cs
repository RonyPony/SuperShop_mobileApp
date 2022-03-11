using System.ComponentModel.DataAnnotations;
using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class BranchDto : BaseDto<Branch, BranchSeedParams>
{
    [Required]
    [MaxLength(80, ErrorMessage = "The name of the branch must be longer than 80 characters")]
    public string Name { get; set; }

    [Required]
    [MaxLength(30, ErrorMessage = "The local code of the branch must be at least 30 characters")]
    public string LocalCode { get; set; }

    [Required(ErrorMessage = "The image url of this branch is required")]
    public string ImageUrl { get; set; }

    [Required(ErrorMessage = "The category of this branch is required")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "The mall id is required")]
    public Guid MallId { get; set; }

    public List<ProductDto>? Products { get; set; }

    public BranchDto()
    {
    }

    public BranchDto(Branch entity) : base(entity)
    {
        Name = entity.Name;
        LocalCode = entity.LocalCode;
        ImageUrl = entity.ImageUrl;
        MallId = entity.MallId;
        CategoryId = entity.CategoryId;
        Products = entity.Products != null ? entity.Products.ToList().ConvertAll(p => new ProductDto(p)) : new List<ProductDto>();
    }

    protected override Branch MakeEntity()
    {
        return new Branch
        {
            Name = Name,
            LocalCode = LocalCode,
            ImageUrl = ImageUrl,
            MallId = MallId,
            CategoryId = CategoryId
        };
    }
}