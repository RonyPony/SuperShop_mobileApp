using System.ComponentModel.DataAnnotations;
using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities;

namespace superShop_API.Database.DTOs;

public class CategoryDto : BaseDto<Category>
{
    [Required]
    [MaxLength(80, ErrorMessage = "The name of the category is required")]
    public string Name { get; set; }
    public CategoryDto()
    {
    }

    public CategoryDto(Category entity) : base(entity) { Name = Entity.Name; }

    protected override Category MakeEntity() { return new Category { Name = Name }; }
}