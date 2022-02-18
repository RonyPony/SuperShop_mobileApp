using Bogus;
using superShop_API.Database.Entities.Base;

namespace superShop_API.Database.Seeders;

public interface ISeeder<Tmodel> where Tmodel : class, IBaseEntity
{
    Faker<Tmodel> SeederDefinition(object? referenceId);
}