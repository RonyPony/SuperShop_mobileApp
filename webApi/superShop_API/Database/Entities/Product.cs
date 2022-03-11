using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Bogus;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;

namespace superShop_API.Database.Entities;

public class ProductSeedParams
{
    public Guid BranchId { get; set; }
    public Guid CategoryId { get; set; }
}

[Table("products")]
public class Product : BaseEntity, ISeeder<Product, ProductSeedParams>
{
    [Required]
    [Column("name", TypeName = "varchar(80)")]
    public string Name { get; set; }

    [Required]
    [Column("code", TypeName = "varchar(32)")]
    public string Code { get; set; }

    [Required]
    [Column("description", TypeName = "varchar(256)")]
    public string Description { get; set; }

    [Required]
    [Column("price", TypeName = "money")]
    public decimal Price { get; set; }

    [Required]
    [Column("stock", TypeName = "integer")]
    public int Stock { get; set; }

    [Required]
    [Column("imageUrl", TypeName = "text")]
    public string ImageUrl { get; set; }

    [Required]
    [Column("branchId")]
    public Guid BranchId { get; set; }

    [Required]
    [Column("categoryId")]
    public Guid CategoryId { get; set; }

    [ForeignKey("branchId")]
    public Branch? Branch { get; set; }

    [ForeignKey("categoryId")]
    public Category? Category { get; set; }

    public IList<ProductOrder>? ProductOrders { get; set; }

    public Faker<Product> SeederDefinition(ProductSeedParams data)
    {
        return new Faker<Product>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Code, Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyz", 32))
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => Convert.ToDecimal(f.Commerce.Price()))
        .RuleFor(p => p.Stock, f => f.Commerce.Random.Int(1, 9999))
        .RuleFor(p => p.BranchId, data.BranchId)
        .RuleFor(p => p.CategoryId, data.CategoryId);
    }
}