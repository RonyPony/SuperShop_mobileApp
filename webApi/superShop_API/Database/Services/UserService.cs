using superShop_API.Database.Entities.Auth;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public interface IUserService : IBaseService<User, Guid>
{

}

public class UserService : BaseService<User, Guid>, IUserService
{
    public UserService(IRepositoryConstructor constructor) : base(constructor)
    {
    }

    public async Task<List<User>> GetAll()
    {
        return (await this.Repository.GetAllAsync()).ToList();
    }

    public override Task<Result<object>> ValidateOnCreateAsync(User entity)
    {
        throw new NotImplementedException();
    }
}