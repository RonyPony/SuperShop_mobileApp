using System.ComponentModel.DataAnnotations;
using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class BranchDto : BaseDto<Branch>
{
    [Required]
    [MaxLength(80, ErrorMessage = "The name of the branch must be longer than 80 characters")]
    public string Name { get; set; }
    [Required]
    public string LocalCode { get; set; }
    [Required]
    public Guid MallId { get; set; }

    public List<ProductDto>? Products { get; set; }

    public BranchDto()
    {
    }

    public BranchDto(Branch entity) : base(entity)
    {
        Name = entity.Name;
        LocalCode = entity.LocalCode;
        MallId = entity.MallId;
        Products = entity.Products != null ? entity.Products.ToList().ConvertAll(p => new ProductDto(p)) : new List<ProductDto>();
    }

    protected override Branch MakeEntity()
    {
        return new Branch
        {
            Name = Name,
            LocalCode = LocalCode,
            MallId = MallId,
        };
    }
}