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

public class Order : BaseEntity<Guid>, ISeeder<Order, Guid, OrderSeedParams>
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
    [Column("totalTax")]
    public double TotalTax { get; set; }

    [Required]
    [Column("totalWhitoutTaxes")]
    public double TotalWhitoutTaxes { get; set; }

    [Required]
    [Column("total")]
    public double Total { get; set; }

    [Required]
    [Column("completed")]
    public bool Completed { get; set; }

    [ForeignKey("branchId")]
    public Branch? Branch { get; set; }

    [ForeignKey("userId")]
    public User? User { get; set; }

    [NotMapped]
    public List<Product>? Products { get; set; }

    public ICollection<ProductOrder>? ProductOrders { get; set; }

    public Faker<Order> SeederDefinition(OrderSeedParams data)
    {
        var total = Convert.ToDouble(new Faker().Commerce.Price());
        var tax = total * 0.18;

        return new Faker<Order>()
        .RuleFor(o => o.BranchId, data.BranchId)
        .RuleFor(o => o.UserId, data.UserId)
        .RuleFor(o => o.Address, f => f.Address.Direction())
        .RuleFor(o => o.TotalTax, tax)
        .RuleFor(o => o.TotalWhitoutTaxes, total)
        .RuleFor(o => o.Total, total + tax)
        .RuleFor(o => o.Completed, f => f.Random.Bool());
    }
}