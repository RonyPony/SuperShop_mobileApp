using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;

namespace superShop_API.Database.Entities
{
    [Table("branches")]
    public class Branch : BaseEntity, ISeeder<Branch>
    {
        [Required]
        [Column("name", TypeName = "varchar(80)")]
        public string Name { get; set; }

        [Required]
        [Column("localCode", TypeName = "varchar(30)")]
        public string LocalCode { get; set; }

        [Required]
        [Column("mallId")]
        public Guid MallId { get; set; }

        [ForeignKey("mallId")]
        public Mall Mall { get; set; }

        public ICollection<Product> Products { get; set; }

        public Faker<Branch> SeederDefinition(object? referenceId)
        {
            return new Faker<Branch>()
            .RuleFor(b => b.Name, f => f.Company.CompanyName())
            .RuleFor(b => b.LocalCode, Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyz", 30))
            .RuleFor(b => b.MallId, referenceId);
        }
    }
}