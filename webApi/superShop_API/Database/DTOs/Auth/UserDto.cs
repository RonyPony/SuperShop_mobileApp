using superShop_API.Database.DTOs.Base;
using superShop_API.Database.Entities.Auth;

namespace superShop_API.Database.DTOs.Auth;

public class UserDto : BaseDto<User>
{
    public UserDto()
    {
    }

    public UserDto(User entity) : base(entity)
    {
        Name = entity.Name;
        LastName = entity.LastName;
        Email = entity.Email;
        UserName = entity.UserName;
        LockoutEnabled = entity.LockoutEnabled;
        RegistrationDate = entity.RegistrationDate;
        CreatedAt = entity.CreatedAt;
        UpdatedAt = entity.UpdatedAt;
    }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    protected override User MakeEntity()
    {
        throw new NotImplementedException();
    }
}