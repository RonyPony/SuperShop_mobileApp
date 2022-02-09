using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace superShop_API.Database.Entities.Base;

 public interface IBaseEntity
    {
        [Key]
        Guid Id { get; set; }

        [Column("createdAt")]
        DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        DateTime UpdatedAt { get; set; }
    }
    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public abstract class BaseIdentityEntity: IdentityUser<Guid>, IBaseEntity{
    public string name {get; set;} 
   
    public string lastName {get; set;}

   public DateTime birthDate {get; set;}

   public string email {get; set;}

   public DateTime registrationDate {get;set;}
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}