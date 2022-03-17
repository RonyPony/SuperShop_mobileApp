using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace superShop_API.Database.Entities.Base;

public interface IBaseEntity<TKey> where TKey : IEquatable<TKey>
{
    [Key]
    TKey Id { get; set; }

    [Column("createdAt")]
    DateTime CreatedAt { get; set; }

    [Column("updatedAt")]
    DateTime UpdatedAt { get; set; }
}
public abstract class BaseEntity<TKey> : IBaseEntity<TKey> where TKey : IEquatable<TKey>
{
    [Key]
    public TKey Id { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}