using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus;
using superShop_API.Database.Entities.Auth;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;

namespace superShop_API.Database.Entities;

public class OrderSeedParams
{
    public Guid BranchId { get; set; }
    public Guid UserId { get; set; }
}

public class Order : BaseEntity, ISeeder<Order, OrderSeedParams>
{
    [Required]
    [Column("branchId")]
    public Guid BranchId { get; set; }

    [Required]
    [Column("userId")]
    public Guid UserId { get; set; }

    [Required]
    [Column("address", TypeName = "varchar(512)")]
    public string Address { get; set; }

    [Required]
    [Column("total")]
    public double Total { get; set; }

    [Required]
    [Column("completed")]
    public bool Completed { get; set; }

    [ForeignKey("branchId")]
    public Branch Branch { get; set; }

    [ForeignKey("userId")]
    public User User { get; set; }

    public IList<ProductOrder> ProductOrders { get; set; }

    public Faker<Order> SeederDefinition(OrderSeedParams data)
    {
        return new Faker<Order>()
        .RuleFor(o => o.BranchId, data.BranchId)
        .RuleFor(o => o.UserId, data.UserId)
        .RuleFor(o => o.Address, f => f.Address.Direction())
        .RuleFor(o => o.Total, f => Convert.ToDouble(f.Commerce.Price()))
        .RuleFor(o => o.Completed, f => f.Random.Bool());
    }
}