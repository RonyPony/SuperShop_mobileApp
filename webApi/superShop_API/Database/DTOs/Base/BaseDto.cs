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

    public Tentity Entity { get { return MakeEntity(); } }

    protected abstract Tentity MakeEntity();
}