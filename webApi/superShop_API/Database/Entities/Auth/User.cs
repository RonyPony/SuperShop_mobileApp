using Bogus;
using Microsoft.AspNetCore.Identity;
using superShop_API.Database.Entities.Base;
using superShop_API.Database.Seeders;

namespace superShop_API.Database.Entities.Auth;

public class User : IdentityUser<Guid>, IBaseEntity<Guid>, ISeeder<User, Guid>
{

    public string Name { get; set; }

    public string LastName { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Faker<User> SeederDefinition(object? referenceId)
    {
        return new Faker<User>()
        .RuleFor(u => u.Name, f => f.Person.FirstName)
        .RuleFor(u => u.LastName, f => f.Person.LastName)
        .RuleFor(u => u.Email, f => f.Person.Email)
        .RuleFor(u => u.RegistrationDate, f => f.Date.Recent());
    }
}