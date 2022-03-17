using System.Linq.Expressions;
using Bogus;
using superShop_API.Database.Entities.Base;

namespace superShop_API.Database.Seeders;

public interface ISeeder<Tmodel, TKey> where Tmodel : class, IBaseEntity<TKey> where TKey : IEquatable<TKey>
{
    Faker<Tmodel> SeederDefinition(object? referenceId);
}

public interface ISeeder<Tmodel, TKey, T> where Tmodel : class, IBaseEntity<TKey> where TKey : IEquatable<TKey>
{
    Faker<Tmodel> SeederDefinition(T data);
}