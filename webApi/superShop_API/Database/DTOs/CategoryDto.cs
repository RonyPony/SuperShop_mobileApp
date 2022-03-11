using System.ComponentModel.DataAnnotations;
using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class CategoryDto : BaseDto<Category>
{
    [Required]
    [StringLength(80, ErrorMessage = "The {0} must be at least {1}")]
    public string Name { get; set; }
    public CategoryDto()
    {
    }

    public CategoryDto(Category entity) : base(entity) { Name = entity.Name; }

    protected override Category MakeEntity() { return new Category { Name = Name }; }
}