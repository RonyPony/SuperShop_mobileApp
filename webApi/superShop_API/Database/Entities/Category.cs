using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;

namespace superShop_API.Database.Entities;

public class Category : BaseEntity, ISeeder<Category>
{
    [Required]
    [Column("name", TypeName = "varchar(80)")]
    public string Name { get; set; }

    public Faker<Category> SeederDefinition(object? referenceId)
    {
        return new Faker<Category>().RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);
    }
}