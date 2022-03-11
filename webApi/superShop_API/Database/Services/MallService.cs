using superShop_API.Database.Entities;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public interface IMallService : IBaseService<Mall>
{
    Task<List<Mall>> GetByName(string name);
}

public class MallService : BaseService<Mall>, IMallService
{
    public MallService(IRepositoryConstructor constructor) : base(constructor)
    {
    }

    public async Task<List<Mall>> GetByName(string name)
    {
        return (await this.Repository.GetAsync(m => m.Name == name)).ToList();
    }

    public async override Task<Result<Object>> ValidateOnCreateAsync(Mall entity)
    {
        try
        {
            var mallFound = (await this.Repository.GetAsync(m => m.Name == entity.Name)).FirstOrDefault();
            if (mallFound == null)
            {
                return Result.Instance().Success("There noting malls in database");
            }
            return Result.Instance().Fail("The requested mall to create exists in database !");
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("An error occurred while making the requested operation", e);
        }
    }
}