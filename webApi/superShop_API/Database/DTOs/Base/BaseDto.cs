using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;

namespace superShop_API.Database.DTOs.Base;

public abstract class BaseDto<Tentity> where Tentity : class, IBaseEntity, ISeeder<Tentity>
{
    public BaseDto()
    {

    }

    public BaseDto(Tentity entity)
    {
        Id = entity.Id;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public Tentity Entity { get { return MakeEntity(); } }

    protected abstract Tentity MakeEntity();
}

public abstract class BaseDto<Tentity, T> where Tentity : class, IBaseEntity, ISeeder<Tentity, T>
{
    public BaseDto()
    {

    }

    public BaseDto(Tentity entity)
    {
        Id = entity.Id;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public Tentity Entity { get { return MakeEntity(); } }

    protected abstract Tentity MakeEntity();
}
