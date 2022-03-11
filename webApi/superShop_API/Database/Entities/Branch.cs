using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;

namespace superShop_API.Database.Entities
{
    public class BranchSeedParams
    {
        public Guid MallId { get; set; }
        public Guid CategoryId { get; set; }
    }

    [Table("branches")]
    public class Branch : BaseEntity, ISeeder<Branch, BranchSeedParams>
    {
        [Required]
        [Column("name", TypeName = "varchar(80)")]
        public string Name { get; set; }

        [Required]
        [Column("imageUrl", TypeName = "text")]
        public string ImageUrl { get; set; }

        [Required]
        [Column("mallId")]
        public Guid MallId { get; set; }

        [ForeignKey("mallId")]
        public Mall? Mall { get; set; }

        [Required]
        [Column("categoryId")]
        public Guid CategoryId { get; set; }

        [ForeignKey("categoryId")]
        public Category? Category { get; set; }

        public ICollection<Product>? Products { get; set; }

        public Faker<Branch> SeederDefinition(BranchSeedParams data)
        {
            return new Faker<Branch>()
            .RuleFor(b => b.Name, f => f.Company.CompanyName())
            .RuleFor(b => b.MallId, data.MallId)
            .RuleFor(b => b.CategoryId, data.CategoryId);
        }
    }
}