using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;

namespace superShop_API.Database.Entities;

public class ProductOrder : BaseEntity, ISeeder<ProductOrder>
{
    [Required]
    [Column("productId")]
    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    [Required]
    [Column("orderId")]
    public Guid OrderId { get; set; }

    public Order Order { get; set; }

    public Faker<ProductOrder> SeederDefinition(object? referenceId)
    {
        throw new NotImplementedException();
    }
}