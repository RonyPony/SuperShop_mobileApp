using System.Text.Json.Serialization;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;

namespace superShop_API.Database.DTOs.Base;

public abstract class BaseDto<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey> where TKey : IEquatable<TKey>
{
    public BaseDto()
    {

    }

    public BaseDto(TEntity entity)
    {
        Id = entity.Id;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    public TKey? Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public TEntity Entity { get { return MakeEntity(); } }

    protected abstract TEntity MakeEntity();
}

public abstract class BaseDto<TEntity, TKey, T> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey, T> where TKey : IEquatable<TKey>
{
    public BaseDto()
    {

    }

    public BaseDto(TEntity entity)
    {
        Id = entity.Id;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    public TKey? Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public TEntity Entity { get { return MakeEntity(); } }

    protected abstract TEntity MakeEntity();
}
